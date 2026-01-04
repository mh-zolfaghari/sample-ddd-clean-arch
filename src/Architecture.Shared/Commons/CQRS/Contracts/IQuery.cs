namespace Architecture.Shared.Commons.CQRS.Contracts;

// This is a marker interface for query requests that expect a response of type TResponse.
public interface IQueryRequest<TResponse> : IMessage
    where TResponse : notnull
{ }

// This is a marker interface for collection query requests that expect a paginated response of type TResponse.
public interface ICollectionQueryRequest<TResponse> : IMessage, IPaginatable, ISortable, ISortableItems
    where TResponse : notnull
{ }

// This interface defines a handler for query requests that expect a response of type TResponse.
public interface IQueryRequestHandler<TQuery, TResponse>
    where TQuery : IQueryRequest<TResponse>
    where TResponse : notnull
{
    Task<Result<TResponse>> Handle(TQuery query, CancellationToken cancellationToken);
}

// This interface defines a handler for collection query requests that expect a paginated response of type TResponse.
public interface ICollectionQueryRequestHandler<TCollectionRequestQuery, TResponse>
    where TCollectionRequestQuery : ICollectionQueryRequest<TResponse>
    where TResponse : notnull
{
    Task<Result<ICollectionActionResponse<TResponse>>> Handle(TCollectionRequestQuery query, CancellationToken cancellationToken);
}