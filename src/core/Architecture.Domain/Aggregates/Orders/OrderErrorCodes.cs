using Architecture.Domain.Aggregates.Orders.ValueObjects;

namespace Architecture.Domain.Aggregates.Orders;

public static class OrderErrorCodes
{
    public static Error NotFound(OrderId domainId) => GlobalErrorCodes.NotFound(domainId, "Order.NotFound", nameof(domainId));
    public static Error NotFound(long id) => GlobalErrorCodes.NotFound(id, "Order.NotFound", nameof(id));
}
