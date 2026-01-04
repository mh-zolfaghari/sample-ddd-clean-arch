namespace Architecture.Shared.Commons.CQRS.Contracts;

// This interface defines a mediator for sending commands, queries, and publishing events.
public interface IMediator
{
    Task<Result.Result> SendAsync(ICommandRequest request, CancellationToken cancellationToken = default);
    Task<Result<TResponse>> SendAsync<TResponse>(ICommandRequest<TResponse> request, CancellationToken cancellationToken = default) where TResponse : notnull;
    Task<Result<TResponse>> SendAsync<TResponse>(IQueryRequest<TResponse> request, CancellationToken cancellationToken = default) where TResponse : notnull;
    Task<Result<ICollectionActionResponse<TResponse>>> SendAsync<TResponse>(ICollectionQueryRequest<TResponse> request, CancellationToken cancellationToken = default) where TResponse : notnull;
    ValueTask PublishAsync<TEvent>(CancellationToken cancellationToken = default, params TEvent[] @events) where TEvent : IEvent;
}
