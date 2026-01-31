namespace Architecture.Shared.Commons.Result;

public record Error
{
    public static readonly Error None = new(string.Empty, ErrorStatus.None, ErrorSeverity.Business);

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
        Code = code;
        Status = status;
        Severity = severity;
        Args = args ?? new Dictionary<string, object?>();
    }
    #endregion

    #region Props

    public string Code { get; private init; }
    public string Domain
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Code))
                return "[Unknown]";

            string? domain = Code.Split('.')[0];
            if (string.IsNullOrWhiteSpace(domain))
                return "[Unknown]";

            return domain;
        }
    }
    public ErrorStatus Status { get; private init; }
    public ErrorSeverity Severity { get; private init; }
    public IDictionary<string, object?> Args { get; init; }

    #endregion

    #region Factory Methods - Simplified with Optional MetaData

    public static Error Validation
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.Validation, severity, args);

    public static Error Unauthorized
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.Unauthorized, severity, args);

    public static Error Forbidden
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.Forbidden, severity, args);

    public static Error NotFound
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.NotFound, severity, args);

    public static Error Conflict
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.Conflict, severity, args);

    public static Error Failure
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Technical,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.Internal, severity, args);

    public static Error UnprocessableEntity
        (
            string code,
            ErrorSeverity severity = ErrorSeverity.Business,
            IDictionary<string, object?>? args = null
        ) => new(code, ErrorStatus.UnprocessableEntity, severity, args);
    #endregion
}