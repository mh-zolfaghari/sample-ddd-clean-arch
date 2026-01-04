namespace Architecture.Application.DTOs.Orders
{
    public sealed record OrderItemDTO
            (
                string ProductName,
                int Quantity,
                decimal Price
            );
}
