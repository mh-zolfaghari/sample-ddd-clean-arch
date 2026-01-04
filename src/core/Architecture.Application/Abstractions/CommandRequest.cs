namespace Architecture.Application.Abstractions;

public abstract record CommandRequest : ICommandRequest
{
    protected CommandRequest()
    { }
}
