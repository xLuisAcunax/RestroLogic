using MediatR;

namespace RestroLogic.Application.Orders.Commands
{
    public record CloseOrderCommand(Guid OrderId) : IRequest;
}
