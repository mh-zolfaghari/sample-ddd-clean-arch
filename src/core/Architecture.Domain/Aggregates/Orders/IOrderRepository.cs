using Architecture.Domain.Aggregates.Orders.ValueObjects;

namespace Architecture.Domain.Aggregates.Orders;

public interface IOrderRepository : IWriteGenericRepository<Order>, IReadGenericRepository<Order, OrderId>
{
}