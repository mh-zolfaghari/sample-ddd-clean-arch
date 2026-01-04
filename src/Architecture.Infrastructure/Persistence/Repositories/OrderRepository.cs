using Architecture.Domain.Orders.ValueObjects;

namespace Architecture.Infrastructure.Persistence.Repositories;

internal sealed class OrderRepository(ApplicationDbContext DbContext) : IOrderRepository
{
    public async Task CreateAsync(Order order, CancellationToken cancellationToken)
    {
        DbContext.Orders.Add(order);
    }

    public async Task DeleteAsync(Order order, CancellationToken cancellationToken)
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
