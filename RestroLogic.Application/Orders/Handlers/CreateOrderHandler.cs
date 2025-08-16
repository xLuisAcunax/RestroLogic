using MediatR;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Orders.Commands;
using RestroLogic.Domain.Entities;
using RestroLogic.Domain.Enums;
using RestroLogic.Domain.Exceptions;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Orders.Handlers
{
    public sealed class CreateOrderHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly AppDbContext _db;
        public CreateOrderHandler(AppDbContext db) => _db = db;

        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken ct)
        {
            var table = await _db.Tables.FirstOrDefaultAsync(t => t.Id == request.TableId, ct)
            ?? throw new DomainException("Table not found.");

            if (table.Status == TableStatus.Blocked)
                throw new DomainException("Cannot open an order on a blocked table.");

            var order = new Order(request.TableId);
            _db.Orders.Add(order);
            await _db.SaveChangesAsync(ct);
            return order.Id;
        }
    }
}
