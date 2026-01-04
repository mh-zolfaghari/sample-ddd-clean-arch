namespace Architecture.Application.Abstractions;

internal sealed class Mediator(IMessageBus messageBus) : IMediator
{
    public Task<Result> SendAsync(ICommandRequest request, CancellationToken cancellationToken = default)
    {
        return messageBus.InvokeAsync<Result>(request, cancellationToken);
    }

    public Task<Result<TResponse>> SendAsync<TResponse>(ICommandRequest<TResponse> request, CancellationToken cancellationToken = default) where TResponse : notnull
    {
        return messageBus.InvokeAsync<Result<TResponse>>(request, cancellationToken);
    }

    public Task<Result<TResponse>> SendAsync<TResponse>(IQueryRequest<TResponse> request, CancellationToken cancellationToken = default) where TResponse : notnull
    {
        return messageBus.InvokeAsync<Result<TResponse>>(request, cancellationToken);
    }

    public Task<Result<ICollectionActionResponse<TResponse>>> SendAsync<TResponse>(ICollectionQueryRequest<TResponse> request, CancellationToken cancellationToken = default) where TResponse : notnull
    {
        return messageBus.InvokeAsync<Result<ICollectionActionResponse<TResponse>>>(request, cancellationToken);
    }

    public async ValueTask PublishAsync<TEvent>(CancellationToken cancellationToken = default, params TEvent[] @events) where TEvent : IEvent
    {
        foreach (TEvent @event in @events)
        {
            await messageBus.PublishAsync(@event);
        }
    }
}
