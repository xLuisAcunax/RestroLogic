using MediatR;
using Microsoft.EntityFrameworkCore;
using RestroLogic.Application.Common.Pagination;
using RestroLogic.Application.Common.Results;
using RestroLogic.Application.Dtos.Sales;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Application.Commands.Sales.GetOrders
{
    public sealed class GetOrdersHandler : IRequestHandler<GetOrdersQuery, Result<PagedResult<OrderSummaryDto>>>
    {
        private readonly AppDbContext _db;
        public GetOrdersHandler(AppDbContext db) => _db = db;


        public async Task<Result<PagedResult<OrderSummaryDto>>> Handle(GetOrdersQuery req, CancellationToken ct)
        {
            var query = _db.Orders.Include(o => o.Items).AsNoTracking().AsQueryable();


            if (string.Equals(req.Status, "open", StringComparison.OrdinalIgnoreCase))
                query = query.Where(o => o.ClosedAt == null);
            else if (string.Equals(req.Status, "closed", StringComparison.OrdinalIgnoreCase))
                query = query.Where(o => o.ClosedAt != null);


            var total = await query.CountAsync(ct);


            var page = Math.Max(1, req.Page);
            var size = Math.Clamp(req.PageSize, 1, 200);


            var data = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * size)
            .Take(size)
            .ToListAsync(ct);


            var items = data.Select(o => new OrderSummaryDto(
            o.Id,
            o.TableNumber,
            o.IsClosed,
            o.CreatedAt,
            o.ClosedAt,
            o.Items.Count,
            o.Total().Amount,
            o.Items.FirstOrDefault()?.UnitPrice.Currency ?? "COP"
            )).ToList();


            return Result<PagedResult<OrderSummaryDto>>.Success(new PagedResult<OrderSummaryDto>(items, page, size, total));
        }
    }
}
