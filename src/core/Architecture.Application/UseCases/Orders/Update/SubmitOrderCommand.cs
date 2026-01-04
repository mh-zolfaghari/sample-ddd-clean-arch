namespace Architecture.Application.UseCases.Orders.Update;

public sealed record SubmitOrderCommand(Guid Id) : CommandRequest;