using Architecture.Domain.Orders.ValueObjects;

namespace Architecture.Domain.Orders.Events;

public sealed record OrderDeletedDomainEvent(OrderId OrderId) : DomainEvent;
