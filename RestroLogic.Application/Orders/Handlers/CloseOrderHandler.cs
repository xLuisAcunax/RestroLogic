using MediatR;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Orders.Commands;
using RestroLogic.Domain.Exceptions;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Orders.Handlers
{
    public sealed class CloseOrderHandler : IRequestHandler<CloseOrderCommand>
    {
        private readonly AppDbContext _db;
        public CloseOrderHandler(AppDbContext db) => _db = db;

        public async Task Handle(CloseOrderCommand request, CancellationToken ct)
        {
            var order = await _db.Orders.Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == request.OrderId, ct)
                ?? throw new DomainException("Order not found.");

            order.Close();
            await _db.SaveChangesAsync(ct);
        }
    }
}
