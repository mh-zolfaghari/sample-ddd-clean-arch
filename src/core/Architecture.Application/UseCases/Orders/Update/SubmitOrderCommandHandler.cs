using Architecture.Domain.Orders;

namespace Architecture.Application.UseCases.Orders.Update;

public sealed class SubmitOrderCommandHandler : ICommandRequestHandler<SubmitOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public SubmitOrderCommandHandler
        (
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork
        )
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var foundedOrder = await _orderRepository.GetAsync(request.Id, cancellationToken);
            if (foundedOrder is null)
                return OrderErrorCodes.NOT_FOUND;

            foundedOrder.SubmitStatus();

            return await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            return OrderErrorCodes.CAN_NOT_SUBMIT;
        }
    }
}
