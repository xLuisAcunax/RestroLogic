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
    }
}
