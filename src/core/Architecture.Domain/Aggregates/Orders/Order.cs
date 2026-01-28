using Architecture.Domain.Aggregates.Orders.Events;
using Architecture.Domain.Aggregates.Orders.Types;
using Architecture.Domain.Aggregates.Orders.ValueObjects;

namespace Architecture.Domain.Aggregates.Orders;

public sealed class Order : AuditableAggregateRoot<OrderId>
{
    private readonly List<OrderItem> _items = new();

    public string OrderNumber { get; private set; }
    public decimal TotalAmount { get; private set; }
    public OrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    private Order() : base(default)
    {
        OrderNumber = string.Empty;
        TotalAmount = 0m;
        Status = OrderStatus.Unknown;
    }

    private Order
        (
            OrderId orderId,
            string orderNumber,
            decimal totalAmount,
            OrderStatus status
        ) : base(orderId)
    {
        OrderNumber = orderNumber;
        TotalAmount = totalAmount;
        Status = status;
    }

    public static Order Create
        (
            string orderNumber,
            decimal totalAmount,
            OrderStatus status
        )
    {
        var order = new Order
            (
                OrderId.New(),
                orderNumber,
                totalAmount,
                status
            );

        order.RaiseDomainEvent(new OrderCreatedDomainEvent(order.DomainId));

        return order;
    }

    public void Remove()
    {
        RaiseDomainEvent(new OrderDeletedDomainEvent(DomainId));
    }

    public void AddItem
        (
            string productName,
            int quantity,
            decimal price
        )
    {
        EnsureDraft();

        var item = OrderItem.Create(Id, productName, quantity, price);

        _items.Add(item);
        RecalculateTotal();
    }

    public void Submit()
    {
        EnsureDraft();

        if (_items.Count == 0)
            throw new InvalidOperationException("Order has no items");

        Status = OrderStatus.Submitted;

        RaiseDomainEvent(new OrderSubmittedDomainEvent(DomainId, TotalAmount));
    }

    private void RecalculateTotal()
    {
        TotalAmount = _items.Sum(x => x.Total());
    }

    private void EnsureDraft()
    {
        if (Status != OrderStatus.Draft)
            throw new InvalidOperationException("Only draft orders can be modified");
    }
}
