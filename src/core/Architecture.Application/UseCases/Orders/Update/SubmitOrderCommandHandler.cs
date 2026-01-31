using Architecture.Domain.Aggregates.Orders;

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
        var foundedOrder = await _orderRepository.GetAsync(request.Id, cancellationToken);
        if (foundedOrder is null)
            return OrderDomainErrorCodes.NotFound(request.Id);

        foundedOrder.Submit();

        return await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
