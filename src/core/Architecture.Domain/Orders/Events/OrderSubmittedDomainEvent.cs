using Architecture.Domain.Orders.ValueObjects;

namespace Architecture.Domain.Orders.Events;

public sealed record OrderSubmittedDomainEvent
    (
        OrderId OrderId,
        decimal TotalAmount
    ) : DomainEvent;
