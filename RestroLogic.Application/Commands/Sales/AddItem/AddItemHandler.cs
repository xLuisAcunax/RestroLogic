using MediatR;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Common.Results;
using RestroLogic.Application.Dtos.Sales;
using RestroLogic.Domain.Sales;
using RestroLogic.Domain.ValueObjects;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Commands.Sales.AddItem
{
    public sealed class AddItemHandler : IRequestHandler<AddItemCommand, Result<OrderDto>>
    {
        private readonly AppDbContext _db;
        public AddItemHandler(AppDbContext db) => _db = db;


        public async Task<Result<OrderDto>> Handle(AddItemCommand req, CancellationToken ct)
        {
            var order = await _db.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == req.OrderId, ct);
            if (order is null)
                return Result<OrderDto>.Failure("Order not found");


            if (order.IsClosed)
                return Result<OrderDto>.Failure("Order is already closed");


            order.AddItem(
            req.MenuItemId,
            req.MenuItemName,
            Money.From(req.UnitPrice, req.Currency),
            Quantity.From(req.Quantity));


            await _db.SaveChangesAsync(ct);


            return Result<OrderDto>.Success(Map(order));
        }


        private static OrderDto Map(Order o)
        {
            var items = o.Items
            .Select(i => new OrderItemDto(
            i.Id,
            i.MenuItemId,
            i.MenuItemName,
            i.Quantity.Value,
            i.UnitPrice.Amount,
            i.UnitPrice.Currency,
            i.Subtotal().Amount))
            .ToList();


            var total = o.Total();
            return new OrderDto(o.Id, o.TableNumber, o.IsClosed, o.CreatedAt, o.ClosedAt, total.Amount, total.Currency, items);
        }
    }
}
