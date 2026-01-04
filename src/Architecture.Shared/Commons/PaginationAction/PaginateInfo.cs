namespace Architecture.Shared.Commons.PaginationAction;

// This record represents pagination information including current page, sorting, next page availability, and total items.
public sealed class PaginateInfo
    (
        Paginatable currentPage,
        Sortable sortingPage,
        bool hasNext,
        long total
    ) : IPaginateInfo
{
    public IPaginatable CurrentPage => currentPage;
    public ISortable SortingPage => sortingPage;
    public bool HasNext => hasNext;
    public long Total => total;
}
