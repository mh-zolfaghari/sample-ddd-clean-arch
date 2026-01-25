using Architecture.Domain.Orders.ValueObjects;
using Architecture.Domain.Shared;

namespace Architecture.Domain.Orders;

public static class OrderErrorCodes
{
    public static Error NotFound(OrderId domainId) => GlobalErrorCodes.NotFound(domainId, "Order.NotFound", nameof(OrderId));
    public static Error NotFound(long id) => GlobalErrorCodes.NotFound(id, "Order.NotFound", nameof(OrderId));
}
