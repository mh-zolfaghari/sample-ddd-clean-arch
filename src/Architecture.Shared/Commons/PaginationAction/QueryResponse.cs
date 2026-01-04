namespace Architecture.Shared.Commons.PaginationAction;

// This record represents a query response containing a single item of type T.
public sealed class QueryResponse<T>(T response) : IResponse<T>
    where T : notnull
{
    public T Response => response;
}

// This record represents a collection query response containing multiple items of type T along with pagination information.
public sealed class CollectionQueryResponse<T>
    (
        IEnumerable<T> response,
        IPaginateInfo info
    ) : ICollectionActionResponse<T>
    where T : notnull
{
    public IEnumerable<T> Response => response;
    public IPaginateInfo Info => info;
}