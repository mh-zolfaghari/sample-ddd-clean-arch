namespace Architecture.Domain.Abstractions.Contracts;

// The interface for all entities that have creation properties.
public interface ICreatableProps
{
    Guid CreatorAccountId { get; }
    DateTime CreatedAt { get; }
    void SetCreated(Guid operatorId, DateTime now);
}

// The interface for all entities that have modification properties.
public interface IModifiableProps
{
    Guid? ModifierAccountId { get; }
    DateTime? ModifiedAt { get; }
    void SetModified(Guid operatorId, DateTime now);
}

// The interface for all entities that have deletion properties.
public interface ISoftDeletableProps
{
    Guid? DeleterAccountId { get; }
    DateTime? DeletedAt { get; }
    void SetDeleted(Guid operatorId, DateTime now);
}


// The interface for all entities that are auditable (have creation and modification and deletion properties for WWW).
public interface IAuditableProps : ICreatableProps, IModifiableProps, ISoftDeletableProps, IRecordState
{ }


// The interface for requesting operator information.
public interface IOperatorRequester
{
    Guid OperatorAccountId { get; }
}