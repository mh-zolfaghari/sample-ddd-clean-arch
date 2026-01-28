using Architecture.Application.Abstractions.Query;
using Architecture.Domain.Aggregates.Orders.Types;

namespace Architecture.Application.DTOs.Orders;

public sealed record OrderDTO
    (
        long TotalCount,
        Guid Id,
        string OrderNumber,
        decimal TotalAmount,
        OrderStatus Status,
        IEnumerable<OrderItemDTO>? Items
    ) : ITotalCountQueryResult;