namespace Architecture.Domain.Abstractions.Contracts;

// The interface for all entities that have a row version for concurrency control.
public interface IRowVersionProps
{
    byte[] RowVersion { get; }
}
