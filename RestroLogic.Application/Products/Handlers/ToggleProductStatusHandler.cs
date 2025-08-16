using MediatR;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Products.Commands;
using RestroLogic.Domain.Exceptions;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Products.Handlers
{
    public sealed class ToggleProductStatusHandler : IRequestHandler<ToggleProductStatusCommand>
    {
        private readonly AppDbContext _db;
        public ToggleProductStatusHandler(AppDbContext db) => _db = db;

        public async Task Handle(ToggleProductStatusCommand request, CancellationToken ct)
        {
            var product = await _db.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId, ct)
                ?? throw new DomainException("Product not found.");

            product.ToggleStatus();
            await _db.SaveChangesAsync(ct);
        }
    }
}
