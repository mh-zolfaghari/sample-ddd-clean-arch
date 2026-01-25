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
            ErrorSeverity severity,
            IDictionary<string, object?>? args = null
        )
    {
        Code = code.ToJsonKey();
        Status = status;
        Severity = severity;
        Args = args ?? new Dictionary<string, object?>();
    }
    #endregion

    #region Props

    // Error code.
    public string Code { get; private init; }

    // Type of the error.
    public ErrorStatus Status { get; private init; }

    // Type of the error severity.
    public ErrorSeverity Severity { get; private init; }

    public IDictionary<string, object?> Args { get; init; }

    #endregion

    #region Factory Methods - Simplified with Optional MetaData

    // Creates a Validation error.
    public static Error Validation
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.Validation, severity, args);

    // Creates an Unauthorized error.
    public static Error Unauthorized
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.Unauthorized, severity, args);

    // Creates a Forbidden error.
    public static Error Forbidden
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.Forbidden, severity, args);

    // Creates a NotFound error.
    public static Error NotFound
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.NotFound, severity, args);

    // Creates a Conflict error.
    public static Error Conflict
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.Conflict, severity, args);

    // Creates a Failure error.
    public static Error Failure
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Technical,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.Internal, severity, args);

    // Creates an UnprocessableEntity error.
    public static Error UnprocessableEntity
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.UnprocessableEntity, severity, args);
    #endregion
}