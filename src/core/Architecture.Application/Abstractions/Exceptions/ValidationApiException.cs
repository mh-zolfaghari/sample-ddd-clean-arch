namespace Architecture.Application.Abstractions.Exceptions;


// Represents validation errors in an API context.
public sealed record ValidationErrorDetail(string Code, string Message);
public sealed record ValidationError(IReadOnlyDictionary<string, IEnumerable<ValidationErrorDetail>> Errors);

// Exception thrown when validation fails in an API context.
public sealed class ValidationApiException(ValidationError errors) : Exception("Validation failed.")
{
    public ValidationError Errors { get; } = errors;
}
