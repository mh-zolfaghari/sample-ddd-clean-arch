namespace Architecture.Domain.Abstractions.Contracts;

// The marker interface for all aggregate roots in the system.
public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
