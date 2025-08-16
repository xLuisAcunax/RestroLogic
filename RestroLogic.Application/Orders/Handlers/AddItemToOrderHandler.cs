using MediatR;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Orders.Commands;
using RestroLogic.Domain.Exceptions;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Orders.Handlers
{
    public sealed class AddItemToOrderHandler : IRequestHandler<AddItemToOrderCommand, Guid>
    {
        private readonly AppDbContext _db;
        public AddItemToOrderHandler(AppDbContext db) => _db = db;

        public async Task<Guid> Handle(AddItemToOrderCommand request, CancellationToken ct)
        {
            var order = await _db.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, ct)
                ?? throw new DomainException("Order not found.");

            var product = await _db.Products.FindAsync(new object?[] { request.ProductId }, ct)
                ?? throw new DomainException("Product not found.");

            // Regla: producto inactivo no se agrega
            if (!product.IsActive) throw new DomainException("Inactive product cannot be added.");

            var added = order.AddItem(product, request.Qty);
            // EF rastrea, pero agregamos explícitamente
            _db.OrderItems.Add(added);
            await _db.SaveChangesAsync(ct);
            return added.Id;
        }
    }
}
