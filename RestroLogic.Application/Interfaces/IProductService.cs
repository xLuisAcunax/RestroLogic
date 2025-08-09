using RestroLogic.Application.Dtos.Products;

namespace RestroLogic.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductListItemDto>> GetAllAsync(bool onlyAvailable = false, CancellationToken ct = default);
        Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Guid> CreateAsync(CreateProductDto dto, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken ct = default);
        Task<bool> UpdateAvailabilityAsync(Guid id, bool isAvailable, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    }
}
