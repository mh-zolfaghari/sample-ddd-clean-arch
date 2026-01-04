namespace Architecture.Shared.Commons.Result;

// This record represents an error with a code,  type, and optional metadata.
public record Error
{
    // Represents no error.
    public static readonly Error None = new(string.Empty, ErrorStatus.None, ErrorSeverity.Business);

    // Implicit conversion from Error to Result.
    public static implicit operator Result(Error error) => Result.Failure(error);

    #region Private Ctor
    private Error
        (
            string code,
            ErrorStatus status,
            ErrorSeverity severity
        )
    {
        Code = code.ToJsonKey();
        Status = status;
        Severity = severity;
    }
    #endregion

    #region Props

    // Error code.
    public string Code { get; private init; }

    // Type of the error.
    public ErrorStatus Status { get; private init; }

    // Type of the error severity.
    public ErrorSeverity Severity { get; private init; }

    #endregion

    #region Factory Methods - Simplified with Optional MetaData

    // Creates a Validation error.
    public static Error Validation(string code, ErrorSeverity severity) => new(code, ErrorStatus.Validation, severity);

    // Creates an Unauthorized error.
    public static Error Unauthorized(string code, ErrorSeverity severity) => new(code, ErrorStatus.Unauthorized, severity);

    // Creates a Forbidden error.
    public static Error Forbidden(string code, ErrorSeverity severity) => new(code, ErrorStatus.Forbidden, severity);

    // Creates a NotFound error.
    public static Error NotFound(string code, ErrorSeverity severity) => new(code, ErrorStatus.NotFound, severity);

    // Creates a Conflict error.
    public static Error Conflict(string code, ErrorSeverity severity) => new(code, ErrorStatus.Conflict, severity);

    // Creates a Failure error.
    public static Error Failure(string code, ErrorSeverity severity) => new(code, ErrorStatus.Failure, severity);

    // Creates an UnprocessableEntity error.
    public static Error UnprocessableEntity(string code, ErrorSeverity severity) => new(code, ErrorStatus.UnprocessableEntity, severity);

    // Creates a TooManyRequests error.
    public static Error TooManyRequests(string code, ErrorSeverity severity) => new(code, ErrorStatus.TooManyRequests, severity);

    // Creates a ServiceUnavailable error.
    public static Error ServiceUnavailable(string code, ErrorSeverity severity) => new(code, ErrorStatus.ServiceUnavailable, severity);
    #endregion
}