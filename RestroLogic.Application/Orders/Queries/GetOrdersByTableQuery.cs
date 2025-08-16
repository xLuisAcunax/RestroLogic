using MediatR;
using RestroLogic.Application.Orders.Dtos;

namespace RestroLogic.Application.Orders.Queries
{
    public record GetOrdersByTableQuery(Guid TableId) : IRequest<OrderSummaryDto>;
}
