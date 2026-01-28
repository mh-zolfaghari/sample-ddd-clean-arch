namespace Architecture.Domain.Aggregates.Orders.ValueObjects;

public sealed class OrderItem : AuditableEntity<Guid>
{
    public long OrderDbId { get; private set; }
    public string ProductName { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
    public Order? Order { get; set; }

    private OrderItem()
        : base(Guid.Empty)
    {
        OrderDbId = default;
        ProductName = string.Empty;
        Quantity = 0;
        Price = 0m;
    }

    private OrderItem
        (
            Guid id,
            long orderDbId,
            string productName,
            int quantity,
            decimal price
        ) : base(id)
    {
        OrderDbId = orderDbId;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
    }

    internal static OrderItem Create
        (
            long orderDbId,
            string productName,
            int quantity,
            decimal price
        )
    {
        return new OrderItem
            (
                Guid.NewGuid(),
                orderDbId,
                productName,
                quantity,
                price
            );
    }

    internal decimal Total() => Price * Quantity;
}
