using MediatR;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Orders.Dtos;
using RestroLogic.Application.Orders.Queries;
using RestroLogic.Domain.Exceptions;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Orders.Handlers
{
    public sealed class GetOrdersByTableHandler : IRequestHandler<GetOrdersByTableQuery, OrderSummaryDto>
    {
        private readonly AppDbContext _db;
        public GetOrdersByTableHandler(AppDbContext db) => _db = db;

        public async Task<OrderSummaryDto> Handle(GetOrdersByTableQuery request, CancellationToken ct)
        {
            var order = await _db.Orders.Include(o => o.Items)
                .Where(o => o.TableId == request.TableId && o.Status == "Open")
                .OrderByDescending(o => o.OpenedAt)
                .FirstOrDefaultAsync(ct);

            if (order is null) throw new DomainException("No open order for this table.");

            var items = await _db.OrderItems
                .Where(i => i.OrderId == order.Id)
                .Join(_db.Products, i => i.ProductId, p => p.Id,
                    (i, p) => new OrderItemDto(p.Id, p.Name, i.Qty, i.UnitPrice, i.LineTotal))
                .ToListAsync(ct);

            return new OrderSummaryDto(order.Id, order.TableId, order.OpenedAt, order.Status, order.Total, items);
        }
    }
}
