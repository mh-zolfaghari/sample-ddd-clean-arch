namespace Architecture.Application.UseCases.Orders;

[ErrorCodeContainer]
internal static class OrderAppErrorCodes
{
    internal static Error OrderNumberMaximumLength(int maxLength)
        => Error.Validation
        (
            code: "Order.OrderNumberMaximumLength",
            args: new Dictionary<string, object?>
            {
                [nameof(maxLength)] = maxLength
            }
        );
}
