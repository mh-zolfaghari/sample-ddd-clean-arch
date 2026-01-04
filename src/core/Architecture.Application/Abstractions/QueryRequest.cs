namespace Architecture.Application.Abstractions;

public abstract record QueryRequest<TResponse> : IQueryRequest<TResponse>
    where TResponse : notnull
{
    protected QueryRequest()
    { }
}

public abstract record CollectionQueryRequest<TResponse> : ICollectionQueryRequest<TResponse>
    where TResponse : notnull
{
    protected CollectionQueryRequest()
    { }

    public int PageIndex { get; init; }
    public int PageSize { get; init; }
    public string[]? SortBy { get; init; }
    public bool? SortDesc { get; init; }

    public string[] SortableItems() => ValidSortFields();

    protected virtual string[] ValidSortFields() => [];
}