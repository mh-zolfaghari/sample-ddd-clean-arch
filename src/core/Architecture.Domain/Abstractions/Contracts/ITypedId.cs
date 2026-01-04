namespace Architecture.Domain.Abstractions.Contracts;

// The marker interface for strongly-typed identifiers
public interface ITypedId
{

}

// The main purpose of this interface is to provide a strongly-typed identifier
public interface ITypedId<out TPrimitive> : ITypedId
{
    TPrimitive Value { get; }
}
