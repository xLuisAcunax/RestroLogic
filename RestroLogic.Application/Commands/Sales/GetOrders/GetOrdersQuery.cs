using MediatR;
using RestroLogic.Application.Common.Pagination;
using RestroLogic.Application.Common.Results;
using RestroLogic.Application.Dtos.Sales;

namespace RestroLogic.Application.Commands.Sales.GetOrders
{
    public sealed record GetOrdersQuery(string? Status = null, int Page = 1, int PageSize = 20)
: IRequest<Result<PagedResult<OrderSummaryDto>>>;
}
