namespace Architecture.Infrastructure.Persistence.Constants;

public static class ConstantMessages
{
    public static readonly Error SaveChangesFailed = Error.Failure("Db.OperationFailed", ErrorSeverity.Technical);
    public static Error UnhandleError(IDictionary<string, object?>? args = null) => Error.Failure("Operation.Failed", ErrorSeverity.Technical, args);
}
