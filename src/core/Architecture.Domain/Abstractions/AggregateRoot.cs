namespace Architecture.Domain.Abstractions;

// The abstract base class for aggregate roots, inheriting from Entity and implementing IAggregateRoot.
public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot
    where TId : struct, ITypedId
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    protected AggregateRoot(TId id)
        : base(id, default)
    { }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}


// The abstract base class for trackable aggregate roots, inheriting from TrackableEntity and implementing IAggregateRoot.
public abstract class AuditableAggregateRoot<TId> : AuditableEntity<TId>, IAggregateRoot
    where TId : struct, ITypedId
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    protected AuditableAggregateRoot(TId id)
        : base(id)
    { }

    protected void RaiseDomainEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents()
        => _domainEvents.Clear();
}