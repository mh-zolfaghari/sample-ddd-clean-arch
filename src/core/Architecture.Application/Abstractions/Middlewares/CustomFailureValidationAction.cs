namespace Architecture.Application.Abstractions.Middlewares;

// This file uses FluentValidation library for validation failures
public class CustomFailureValidationAction<T> : IFailureAction<T>
{
    // Throws a custom ValidationApiException with detailed validation errors
    public void Throw(T message, IReadOnlyList<ValidationFailure> failures)
    {
        var errors = failures
            .GroupBy(e => e.PropertyName)
            .ToDictionary
                (
                    g => g.Key,
                    g => g.Select(e => new ValidationErrorDetail(e.ErrorCode.Replace("Validator", string.Empty, StringComparison.OrdinalIgnoreCase).ToJsonKey(), e.ErrorMessage)).Distinct()
                );

        throw new ValidationApiException(new ValidationError(errors));
    }
}
