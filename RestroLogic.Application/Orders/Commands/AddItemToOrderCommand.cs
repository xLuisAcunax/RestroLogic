using MediatR;

namespace RestroLogic.Application.Orders.Commands
{
    public record AddItemToOrderCommand(Guid OrderId, Guid ProductId, int Qty) : IRequest<Guid>;
}
