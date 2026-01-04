using Architecture.Domain.Orders;

namespace Architecture.Application.UseCases.Orders.Delete;

public sealed class DeleteOrderCommandHandler : ICommandRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteOrderCommandHandler
        (
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork
        )
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var foundedOrder = await _orderRepository.GetAsync(request.Id, cancellationToken);
            if (foundedOrder is null)
                return OrderErrorCodes.NOT_FOUND;

            foundedOrder.FullDelete();
            await _orderRepository.DeleteAsync(foundedOrder, cancellationToken);

            return await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return OrderErrorCodes.CAN_NOT_DELETE;
        }
    }
}
