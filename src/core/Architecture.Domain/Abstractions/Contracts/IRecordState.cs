namespace Architecture.Domain.Abstractions.Contracts;

// The interface for all entities that have a record state.
public interface IRecordState
{
    RecordState RecordState { get; }
}
