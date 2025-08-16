using MediatR;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Products.Dtos;
using RestroLogic.Application.Products.Queries;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Products.Handlers
{
    public sealed class GetActiveProductsHandler : IRequestHandler<GetActiveProductsQuery, IReadOnlyList<ProductDto>>
    {
        private readonly AppDbContext _db;
        public GetActiveProductsHandler(AppDbContext db) => _db = db;

        public async Task<IReadOnlyList<ProductDto>> Handle(GetActiveProductsQuery request, CancellationToken ct)
        {
            return await _db.Products
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.IsActive))
                .ToListAsync(ct);
        }
    }
}
