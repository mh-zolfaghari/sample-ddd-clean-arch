using Architecture.Application.Abstractions.Extensions;
using Architecture.Application.DTOs.Orders;

namespace Architecture.Application.UseCases.Orders.Filter;

public sealed class FilterOrdersQueryValidator : CollectionQueryRequestValidator<FilterOrdersQuery, OrderDTO>
{
    protected override void Validations()
    {
        RuleFor(x => x.OrderNumber)
            .MaximumLength(12).WithResultErrorValidation(OrderAppErrorCodes.OrderNumberMaximumLength(12));
    }
}
