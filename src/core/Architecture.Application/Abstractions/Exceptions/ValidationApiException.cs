namespace Architecture.Application.Abstractions.Exceptions;

public sealed record ValidationError(IReadOnlyDictionary<string, IEnumerable<Error>> ErrorCodes);

// Exception thrown when validation fails in an API context.
public sealed class ValidationApiException(ValidationError errors) : Exception("Validation failed.")
{
    public ValidationError Errors { get; } = errors;
}
