using Architecture.Application.DTOs.Orders;
using Architecture.Domain.Orders.Types;

namespace Architecture.Application.UseCases.Orders.Filter
{
    public sealed record FilterOrdersQuery
        (
            string? OrderNumber = default,
            OrderStatus[]? StatusTypes = default
        ) : CollectionQueryRequest<OrderDTO>
    {
        // Define valid sort fields for orders
        protected override string[] ValidSortFields()
            => [
                   nameof(OrderNumber),
                   "Status"
               ];

        // Validator for FilterOrdersQuery
        public sealed class FilterOrdersQueryValidator : CollectionQueryRequestValidator<FilterOrdersQuery, OrderDTO>
        {
            protected override void Validations()
            {
                RuleFor(x => x.OrderNumber)
                    .MaximumLength(12).WithMessage("The OrderNumber value cannot exceed 20 characters.");
            }
        }
    }
}
