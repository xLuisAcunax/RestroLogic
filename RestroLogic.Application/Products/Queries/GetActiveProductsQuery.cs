using MediatR;
using RestroLogic.Application.Products.Dtos;

namespace RestroLogic.Application.Products.Queries
{
    public record GetActiveProductsQuery() : IRequest<IReadOnlyList<ProductDto>>;
}
