using Architecture.Application.Abstractions.Extensions;
using Architecture.Application.DTOs.Orders;

namespace Architecture.Application.UseCases.Orders.Filter
{
    public sealed class FilterOrdersQueryHandler : ICollectionQueryRequestHandler<FilterOrdersQuery, OrderDTO>
    {
        public async Task<Result<ICollectionActionResponse<OrderDTO>>> Handle(FilterOrdersQuery query, CancellationToken cancellationToken)
        {
            //var tSqlQuery = TSqlQueryBuilder.CreateQuery()
            //    .Initialize
            //    (
            //        $"""
            //        SELECT 
            //            ord.{nameof(OrderDTO.Id)},
            //            ord.{nameof(OrderDTO.OrderNumber)},
            //            ord.{nameof(OrderDTO.Status)},
            //            ord.{nameof(OrderDTO.TotalAmount)},
            //        FROM [dbo].[Orders] ord
            //        """
            //    )
            //    .Where(!string.IsNullOrWhiteSpace(query.OrderNumber), DbUtil.Like($"ord.{nameof(OrderDTO.OrderNumber)}", query.OrderNumber!))
            //    .AND_Where(query.StatusTypes?.Length > 0, DbUtil.In($"ord.{nameof(OrderDTO.Status)}", query.StatusTypes!))
            //    .OR_Where(DbUtil.NotIn($"ord.{nameof(OrderDTO.OrderNumber)}", "216654654", "65454655", "54654658"))
            //    .IgnoreSoftDeletedItems("ord")
            //    .AppendPagination(query, new Sortable([$"ord.{nameof(OrderDTO.Id)}"], true))
            //    .ToString();

            //return null!;

            return new List<OrderDTO>
            {
                new(1, Guid.NewGuid(), "5ds4d5s4d", 1200.0M, Domain.Aggregates.Orders.Types.OrderStatus.Draft, null)
            }.ToPaginatedResult(query);
        }
    }
}
