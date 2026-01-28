namespace Architecture.Domain.Aggregates.Orders.ValueObjects;

public readonly record struct OrderId(Guid Value) : ITypedId<Guid>
{
    public static OrderId New() 
        => new(Guid.CreateVersion7());

    public override string ToString() 
        => Value.ToString();

    public static implicit operator OrderId(Guid value) 
        => new(value);

    public static implicit operator Guid(OrderId orderId)
        => orderId.Value;
}
