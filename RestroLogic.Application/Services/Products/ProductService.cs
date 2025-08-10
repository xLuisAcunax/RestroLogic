using RestroLogic.Application.Dtos.Products;
using RestroLogic.Application.Interfaces;
using RestroLogic.Domain.Entities;
using RestroLogic.Domain.Interfaces;

namespace RestroLogic.Application.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public async Task<IEnumerable<ProductListItemDto>> GetAllAsync(bool onlyAvailable = false, CancellationToken ct = default)
        {
            var src = onlyAvailable
                ? await _productRepository.GetAllActiveAsync()
                : await _productRepository.GetAllAsync();

            return src.Select(p => new ProductListItemDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                IsAvailable = p.IsAvailable
            });
        }

        public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var p = await _productRepository.GetByIdAsync(id, ct);
            if (p is null) return null;

            return new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                IsAvailable = p.IsAvailable
            };
        }

        public async Task<Guid> CreateAsync(CreateProductDto dto, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(dto.Name)) throw new ArgumentException("Name is required.");
            if (dto.Price <= 0) throw new ArgumentException("Price must be greater than zero.");

            var product = new Product(dto.Name, dto.Description ?? string.Empty, dto.Price);
            product.SetImageUrl(dto.ImageUrl); 
            await _productRepository.AddAsync(product, ct);
            return product.Id;
        }

        public async Task<bool> UpdateAsync(Guid id, UpdateProductDto dto, CancellationToken ct = default)
        {
            var p = await _productRepository.GetByIdAsync(id, ct);
            if (p is null) return false;

            p.Rename(dto.Name);
            p.ChangeDescription(dto.Description);
            p.ChangePrice(dto.Price);
            p.SetImageUrl(dto.ImageUrl);
            if (dto.IsAvailable) p.MarkAsAvailable(); else p.MarkAsUnavailable();

            await _productRepository.UpdateAsync(p, ct);
            return true;
        }

        public async Task<bool> UpdateAvailabilityAsync(Guid id, bool isAvailable, CancellationToken ct = default)
        {
            var p = await _productRepository.GetByIdAsync(id, ct);
            if (p is null) return false;

            if (isAvailable) p.MarkAsAvailable(); else p.MarkAsUnavailable();
            await _productRepository.UpdateAsync(p, ct);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var p = await _productRepository.GetByIdAsync(id, ct);
            if (p is null) return false;

            await _productRepository.DeleteByIdAsync(id, ct); 
            return true;
        }
    }
}
