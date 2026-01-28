using Architecture.Domain.Aggregates.Orders.ValueObjects;

namespace Architecture.Domain.Aggregates.Orders.Events;

public sealed record OrderCreatedDomainEvent(OrderId OrderId) : DomainEvent;
