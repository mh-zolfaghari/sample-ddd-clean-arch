namespace Architecture.Domain.Abstractions;

// The base class for all entities in the system.
public abstract class Entity<TId> : IEntity<TId>, IRowVersionProps
    where TId : notnull
{
    public long Id { get; private set; }
    public TId DomainId { get; protected init; }
    public byte[] RowVersion { get; private set; } = DefaultValues.DefaultRowVersion;

    protected Entity(TId domainId, long id)
    {
        DomainId = domainId;
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TId> other)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        return EqualityComparer<TId>.Default.Equals(DomainId, other.DomainId);
    }

    public override int GetHashCode()
        => EqualityComparer<TId>.Default.GetHashCode(DomainId);
}


// The abstract base class for trackable entities, inheriting from Entity and implementing IAuditableProps.
public abstract class AuditableEntity<TId> : Entity<TId>, IAuditableProps
    where TId : notnull
{
    protected AuditableEntity(TId id)
        : base(id, default)
    {
        RecordState = RecordState.Unknown;
    }

    public Guid CreatorAccountId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Guid? ModifierAccountId { get; private set; }
    public DateTime? ModifiedAt { get; private set; }
    public Guid? DeleterAccountId { get; private set; }
    public DateTime? DeletedAt { get; private set; }
    public RecordState RecordState { get; private set; }

    void ICreatableProps.SetCreated(Guid operatorId, DateTime now)
    {
        CreatorAccountId = operatorId;
        CreatedAt = now;
        RecordState = RecordState.Added;
    }

    void IModifiableProps.SetModified(Guid operatorId, DateTime now)
    {
        ModifierAccountId = operatorId;
        ModifiedAt = now;
        RecordState = RecordState.Updated;
    }

    void ISoftDeletableProps.SetDeleted(Guid operatorId, DateTime now)
    {
        DeleterAccountId = operatorId;
        DeletedAt = now;
        RecordState = RecordState.Deleted;
    }
}