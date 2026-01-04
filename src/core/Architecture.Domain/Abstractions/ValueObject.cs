namespace Architecture.Domain.Abstractions;

// Base class for value objects
public abstract class ValueObject : IValueObject
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    // Overrides for equality comparison
    public override bool Equals(object? obj)
    {
        if (obj is not ValueObject other)
            return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    // Override for hash code generation
    public override int GetHashCode()
    {
        return GetEqualityComponents()
            .Aggregate(1, (current, obj) =>
            {
                unchecked
                {
                    return current * 23 + (obj?.GetHashCode() ?? 0);
                }
            });
    }
}
