using MediatR;
using RestroLogic.Application.Common.Results;

namespace RestroLogic.Application.Commands.Sales.CloseOrder
{
    public sealed record CloseOrderCommand(Guid OrderId) : IRequest<Result<decimal>>; // devuelve total
}
