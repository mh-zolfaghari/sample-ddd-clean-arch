namespace Architecture.Domain.Abstractions.Contracts;

// The IDomainEvent interface represents a domain event in the system.
public interface IDomainEvent : IEvent
{
    DateTime OccurredOn { get; }
}
