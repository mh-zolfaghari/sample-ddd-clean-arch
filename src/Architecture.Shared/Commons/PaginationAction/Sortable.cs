namespace Architecture.Shared.Commons.PaginationAction;

// This record represents sorting parameters with sort by field and sort direction.
public sealed class Sortable
    (
        string[]? sortBy,
        bool? sortDesc
    ) : ISortable
{
    public string[]? SortBy => sortBy;
    public bool? SortDesc => sortDesc;
}
