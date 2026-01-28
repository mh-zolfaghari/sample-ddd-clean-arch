using Architecture.Domain.Aggregates.Orders;
using Architecture.Domain.Aggregates.Orders.Types;

namespace Architecture.Application.UseCases.Orders.Create;

public sealed class CreateOrderCommandHandler : ICommandRequestHandler<CreateOrderCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateOrderCommandHandler
        (
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork
        )
    {
        _orderRepository = orderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = Order.Create
        (
            orderNumber: $"ORD-{DateTime.UtcNow.ToString("yyyyMMdd")}-{new Random().NextInt64(100, 100000)}",
            totalAmount: 56451.54M,
            status: OrderStatus.Draft
        );

        order.AddItem
            (
                productName: $"PRD-{new Random().NextInt64(10000, 999999)}",
                quantity: new Random().Next(1, 100),
                price: 1120.12M
            );

        _orderRepository.Create(order);
        return await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
