namespace Architecture.Application.Abstractions.Middlewares;

// This file uses FluentValidation library for validation failures
public class CustomFailureValidationAction<T> : IFailureAction<T>
{
    // Throws a custom ValidationApiException with detailed validation errors
    public void Throw(T message, IReadOnlyList<ValidationFailure> failures)
    {
        var resultErrors = failures
            .GroupBy(e => e.PropertyName)
            .ToDictionary
                (
                    g => g.Key,
                    g => g.Select(e =>
                    {
                        if (e.CustomState is not null)
                        {
                            if (e.CustomState is Error error)
                                return error;
                            throw new ArgumentException("Invalid CustomState object from Validator");
                        }

                        if (!string.IsNullOrWhiteSpace(e.ErrorCode))
                            return Error.Validation(e.ErrorCode.Replace("Validator", string.Empty, StringComparison.OrdinalIgnoreCase));

                        return Error.Validation("Validation.UnhandledErrorMessage", ErrorSeverity.Technical);
                    }).DistinctBy(x => x.Code)
                );

        throw new ValidationApiException(new ValidationError(resultErrors));
    }
}
