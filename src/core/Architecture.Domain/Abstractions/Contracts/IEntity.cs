namespace Architecture.Domain.Abstractions.Contracts;

// The marker interface for all entities in the system.
public interface IEntity
{
    long Id { get; }
}

// The generic base interface for all entities in the system.
public interface IEntity<out TId> : IEntity
    where TId : notnull
{
    TId DomainId { get; }
}