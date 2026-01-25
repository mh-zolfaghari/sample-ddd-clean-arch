using Architecture.Domain.Orders.ValueObjects;

namespace Architecture.Infrastructure.Persistence.Repositories;

internal sealed class OrderRepository(ApplicationDbContext DbContext) : IOrderRepository
{
    public void Create(Order order)
    {
        DbContext.Orders.Add(order);
    }

    public void Delete(Order order)
    {
        DbContext.OrderItems.RemoveRange(order.Items);
        DbContext.Orders.Remove(order);
    }

    public async Task<Order?> GetAsync(OrderId domainId, CancellationToken cancellationToken)
    {
        return await DbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.DomainId == domainId, cancellationToken);
    }

    public async Task<Order?> GetAsync(long id, CancellationToken cancellationToken)
    {
        return await DbContext.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
}
