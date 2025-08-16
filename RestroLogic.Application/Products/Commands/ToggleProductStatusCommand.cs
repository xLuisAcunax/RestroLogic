using MediatR;

namespace RestroLogic.Application.Products.Commands
{
    public record ToggleProductStatusCommand(Guid ProductId) : IRequest;
}
