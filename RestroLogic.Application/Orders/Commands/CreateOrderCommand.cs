using MediatR;

namespace RestroLogic.Application.Orders.Commands
{
    public record CreateOrderCommand(Guid TableId) : IRequest<Guid>;
}
