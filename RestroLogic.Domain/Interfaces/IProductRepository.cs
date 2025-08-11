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

        Task<(IEnumerable<Product> Items, int Total)> SearchAsync(
            string? search,
            bool? onlyAvailable,
            string? sortBy,
            bool desc,
            int page,
            int pageSize,
            CancellationToken ct = default);
    }
}
