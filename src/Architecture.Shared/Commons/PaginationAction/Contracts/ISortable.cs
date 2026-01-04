namespace Architecture.Shared.Commons.PaginationAction.Contracts;

// This interface defines the contract for sortable entities.
public interface ISortable
{
    string[]? SortBy { get; }
    bool? SortDesc { get; }
}
