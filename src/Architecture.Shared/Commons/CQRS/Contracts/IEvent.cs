namespace Architecture.Shared.Commons.CQRS.Contracts;

// This is a marker interface for event messages.
public interface IEvent
{
}

// This interface defines a handler for event messages of type TEvent.
public interface IEventHandler<in TEvent>
    where TEvent : class, IEvent
{
    Task Handle(TEvent @event, CancellationToken cancellationToken);
}