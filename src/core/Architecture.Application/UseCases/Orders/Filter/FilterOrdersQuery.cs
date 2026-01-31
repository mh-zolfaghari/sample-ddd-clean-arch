using Architecture.Application.DTOs.Orders;
using Architecture.Domain.Aggregates.Orders.Types;

namespace Architecture.Application.UseCases.Orders.Filter
{
    public sealed record FilterOrdersQuery
        (
            string? OrderNumber = default,
            OrderStatus[]? StatusTypes = default
        ) : CollectionQueryRequest<OrderDTO>
    {
        // Define valid sort fields for orders
        protected override string[] ValidSortFields()
            => [
                   nameof(OrderNumber),
                   "Status"
               ];
    }
}
