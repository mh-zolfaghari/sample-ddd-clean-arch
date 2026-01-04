namespace Architecture.Shared.Commons.PaginationAction;

// This record represents pagination parameters with page index and page size.
public sealed class Paginatable
    (
        int pageIndex,
        int pageSize
    ) : IPaginatable
{
    public int PageIndex => pageIndex;
    public int PageSize => pageSize;
}
