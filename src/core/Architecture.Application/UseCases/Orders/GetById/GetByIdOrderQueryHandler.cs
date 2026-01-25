using Architecture.Application.DTOs.Orders;

namespace Architecture.Application.UseCases.Orders.GetById;

public sealed class GetByIdOrderQueryHandler : IQueryRequestHandler<GetByIdOrderQuery, OrderDTO>
{
    public async Task<Result<OrderDTO>> Handle(GetByIdOrderQuery query, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        return null!;
    }
}
