using Architecture.Application.DTOs.Orders;

namespace Architecture.Application.UseCases.Orders.GetById;

public sealed record GetByIdOrderQuery(Guid Id) : IQueryRequest<OrderDTO>;
