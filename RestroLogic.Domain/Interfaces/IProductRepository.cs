using RestroLogic.Domain.Entities;

namespace RestroLogic.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetAllActiveAsync();
        Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task AddAsync(Product order, CancellationToken cancellationToken = default);
        Task UpdateAsync(Product order, CancellationToken cancellationToken = default);
        Task DeleteByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
