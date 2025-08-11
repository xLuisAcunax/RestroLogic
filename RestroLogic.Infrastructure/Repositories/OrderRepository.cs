using Microsoft.EntityFrameworkCore;
using RestroLogic.Domain.Entities;
using RestroLogic.Domain.Interfaces;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context) { _context = context; }

        public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
        {
            await _context.Orders.AddAsync(order, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByCustomerIdAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .Include(o => o.Items)
                .ToListAsync(cancellationToken);
        }

        public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
        {
            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Order> Items, int Total)> SearchAsync(
        Guid? customerId, DateTime? from, DateTime? to,
        string? sortBy, bool desc, int page, int pageSize, CancellationToken ct = default)
        {
            var q = _context.Orders
                       .Include(o => o.Items)
                       .AsNoTracking()
                       .AsQueryable();

            if (customerId.HasValue)
                q = q.Where(o => o.CustomerId == customerId.Value);

            if (from.HasValue)
                q = q.Where(o => o.CreatedAt >= from.Value);

            if (to.HasValue)
                q = q.Where(o => o.CreatedAt <= to.Value);

            sortBy = (sortBy ?? "date").ToLowerInvariant();
            var isDesc = desc;

            q = sortBy switch
            {
                "id" => isDesc ? q.OrderByDescending(o => o.Id) : q.OrderBy(o => o.Id),
                _ => isDesc ? q.OrderByDescending(o => o.CreatedAt) : q.OrderBy(o => o.CreatedAt),
            };

            var total = await q.CountAsync(ct);
            var skip = (Math.Max(1, page) - 1) * Math.Max(1, pageSize);
            var items = await q.Skip(skip).Take(pageSize).ToListAsync(ct);

            return (items, total);
        }
    }
}
