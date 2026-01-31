using Architecture.Domain.Aggregates.Orders.ValueObjects;

namespace Architecture.Domain.Aggregates.Orders;

[ErrorCodeContainer]
public static class OrderDomainErrorCodes
{
    public static Error NotFound(OrderId domainId) => GlobalDomainErrorCodes.NotFound(domainId, "Order.NotFound", nameof(domainId));
    public static Error NotFound(long id) => GlobalDomainErrorCodes.NotFound(id, "Order.NotFound", nameof(id));
}
