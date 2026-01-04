namespace Architecture.Shared.Commons.CQRS.Contracts;

// This is a marker interface for command requests.
public interface ICommandRequest : IMessage
{
}

// This interface represents a command request that expects a response of type TResponse.
public interface ICommandRequest<TResponse> : ICommandRequest
    where TResponse : notnull
{
}

// This interface defines a handler for command requests without a response.
public interface ICommandRequestHandler<TCommand>
    where TCommand : ICommandRequest
{
    Task<Result.Result> Handle(TCommand command, CancellationToken cancellationToken);
}

// This interface defines a handler for command requests that expect a response of type TResponse.
public interface ICommandRequestHandler<TCommand, TResponse>
    where TCommand : ICommandRequest<TResponse>
    where TResponse : notnull
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}