using Architecture.Domain.Orders.ValueObjects;

namespace Architecture.Domain.Orders.Events;

public sealed record OrderCreatedDomainEvent(OrderId OrderId) : DomainEvent;
