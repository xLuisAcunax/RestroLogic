using MediatR;
using RestroLogic.Application.Common.Results;

namespace RestroLogic.Application.Commands.Sales.CreateOrder
{
    public record CreateOrderCommand(int TableNumber) : IRequest<Result<Guid>>;
}
