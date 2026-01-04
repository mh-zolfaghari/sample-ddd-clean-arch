namespace Architecture.Infrastructure.Persistence.Constants;

internal class ConstantMessages
{
    internal static readonly Error SaveChangesFailed = Error.Failure(nameof(SaveChangesFailed), ErrorSeverity.Technical);
}
