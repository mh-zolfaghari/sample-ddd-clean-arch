namespace Architecture.Application.UseCases.Orders.Delete;

public sealed record DeleteOrderCommand(Guid Id) : CommandRequest;
