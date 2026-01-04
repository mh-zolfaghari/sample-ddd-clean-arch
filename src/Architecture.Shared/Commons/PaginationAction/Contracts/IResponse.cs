namespace Architecture.Shared.Commons.PaginationAction.Contracts;

// This interface defines the contract for a generic response.
public interface IResponse
{ }

// This interface defines the contract for a typed response.
public interface IResponse<out T> : IResponse
    where T : notnull
{
    T Response { get; }
}

// This interface defines the contract for a collection action response with pagination info.
public interface ICollectionActionResponse<out T> : IResponse<IEnumerable<T>>
    where T : notnull
{
    IPaginateInfo Info { get; }
}