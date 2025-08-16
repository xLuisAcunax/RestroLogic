using MediatR;

namespace RestroLogic.Application.Products.Commands
{
    public record CreateProductCommand(string Name, string? Description, decimal Price) : IRequest<Guid>;
}
