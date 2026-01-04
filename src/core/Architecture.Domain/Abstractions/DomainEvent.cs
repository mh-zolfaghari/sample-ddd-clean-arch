namespace Architecture.Domain.Abstractions;

// The DomainEvent abstract record provides a base implementation of the IDomainEvent interface.
public abstract record DomainEvent : IDomainEvent
{
    private readonly DateTime _occurredOn;

    protected DomainEvent()
    {
        _occurredOn = DateTime.UtcNow;
    }

    public DateTime OccurredOn => _occurredOn;
}