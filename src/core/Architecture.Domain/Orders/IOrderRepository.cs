using Architecture.Domain.Orders.ValueObjects;

namespace Architecture.Domain.Orders;

public interface IOrderRepository : IWriteGenericRepository<Order>, IReadGenericRepository<Order, OrderId>
{
}