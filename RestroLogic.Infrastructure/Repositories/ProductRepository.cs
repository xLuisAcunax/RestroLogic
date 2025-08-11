using Microsoft.EntityFrameworkCore;
using RestroLogic.Domain.Entities;
using RestroLogic.Domain.Interfaces;
using RestroLogic.Infrastructure.Persistence;

namespace RestroLogic.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            // Lectura sin tracking para performance en listados
            return await _db.Products.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllActiveAsync()
        {
            return await _db.Products
                .AsNoTracking()
                .Where(p => p.IsAvailable)
                .ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _db.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _db.Products.AddAsync(product, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var product = _db.Products.Find(id) ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
            _db.Products.Remove(product);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task<(IEnumerable<Product> Items, int Total)> SearchAsync(
            string? search, bool? onlyAvailable, string? sortBy, bool desc,
            int page, int pageSize, CancellationToken ct = default)
        {
            var query = _db.Products.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                query = query.Where(p => p.Name.ToLower().Contains(term)
                                       || (p.Description != null && p.Description.ToLower().Contains(term)));
            }

            if (onlyAvailable is true)
                query = query.Where(p => p.IsAvailable);

            // Orden
            sortBy = (sortBy ?? "name").ToLowerInvariant();
            var isDesc = desc;

            query = (sortBy) switch
            {
                "price" => isDesc ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "id" => isDesc ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id),
                _ => isDesc ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            };

            var total = await query.CountAsync(ct);

            // Paginación (1-based)
            var skip = (Math.Max(1, page) - 1) * Math.Max(1, pageSize);
            var items = await query.Skip(skip).Take(pageSize).ToListAsync(ct);

            return (items, total);
        }
    }
}
