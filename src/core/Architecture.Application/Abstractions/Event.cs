namespace Architecture.Application.Abstractions;

public abstract record Event : IEvent
{
    protected Event()
    { }
}