namespace Architecture.Shared.Commons.PaginationAction.Contracts;

// This interface defines the contract for paginatable entities.
public interface IPaginatable
{
    int PageIndex { get; }
    int PageSize { get; }
}
