namespace Architecture.Shared.Commons.PaginationAction.Contracts;

// This interface defines the contract for pagination information.
public interface IPaginateInfo
{
    IPaginatable CurrentPage { get; }
    ISortable SortingPage { get; }
    bool HasNext { get; }
    long Total { get; }
}