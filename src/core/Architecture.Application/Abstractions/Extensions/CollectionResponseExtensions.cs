using Architecture.Application.Abstractions.Query;
using Architecture.Shared.Commons.PaginationAction;

namespace Architecture.Application.Abstractions.Extensions;

public static class CollectionResponseExtensions
{
    public static Result<ICollectionActionResponse<T>> ToPaginatedResult<T>
        (
            this IEnumerable<T>? source,
            ICollectionQueryRequest<T> collectionQueryRequest
        ) where T : notnull, ITotalCountQueryResult, new()
    {
        return CreateResponse(source, collectionQueryRequest, source?.FirstOrDefault()?.TotalCount ?? 0);
    }

    public static Result<ICollectionActionResponse<T>> ToPaginatedResult<T>
        (
            this IEnumerable<T>? source,
            ICollectionQueryRequest<T> collectionQueryRequest,
            long total
        ) where T : notnull
    {
        return CreateResponse(source, collectionQueryRequest, total);
    }

    private static Result<ICollectionActionResponse<T>> CreateResponse<T>
        (
            this IEnumerable<T>? source,
            ICollectionQueryRequest<T> collectionQueryRequest,
            long total
        ) where T : notnull
    {
        source ??= [];
        bool hasNext = source.Count() > collectionQueryRequest.PageSize;

        return new CollectionQueryResponse<T>
            (
                response: hasNext ? source.Take(collectionQueryRequest.PageSize) : source,
                info: new PaginateInfo
                    (
                        currentPage: new Paginatable
                            (
                                pageIndex: collectionQueryRequest.PageIndex,
                                pageSize: collectionQueryRequest.PageSize
                            ),
                        sortingPage: new Sortable
                            (
                                sortBy: collectionQueryRequest.SortBy,
                                sortDesc: collectionQueryRequest.SortDesc
                            ),
                        hasNext: hasNext,
                        total: total
                    )
            );
    }
}
