namespace Architecture.Application.UseCases.Orders;

internal static class OrderErrorCodes
{
    internal static Error NOT_FOUND = Error.NotFound(nameof(NOT_FOUND), ErrorSeverity.Business);
    internal static Error CAN_NOT_SUBMIT = Error.Failure(nameof(CAN_NOT_SUBMIT), ErrorSeverity.Business);
    internal static Error CAN_NOT_CREATE = Error.Failure(nameof(CAN_NOT_CREATE), ErrorSeverity.Business);
    internal static Error CAN_NOT_DELETE = Error.Failure(nameof(CAN_NOT_DELETE), ErrorSeverity.Business);
}
