using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestroLogic.Application.Dtos.Products;
using RestroLogic.Application.Interfaces;
using RestroLogic.Domain.Interfaces;
using RestroLogic.Infrastructure.Interfaces;

namespace RestroLogic.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service)
        {
            _service = service;
        }

        // GET: api/products?onlyAvailable=true
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductListItemDto>>> GetAll([FromQuery] bool onlyAvailable = false, CancellationToken ct = default)
        {
            var items = await _service.GetAllAsync(onlyAvailable, ct);
            return Ok(items);
        }

        // GET: api/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDto>> GetById(Guid id, CancellationToken ct = default)
        {
            var item = await _service.GetByIdAsync(id, ct);
            if (item is null) return NotFound();
            return Ok(item);
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateProductDto dto, CancellationToken ct = default)
        {
            var id = await _service.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        // PUT: api/products/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto, CancellationToken ct = default)
        {
            var ok = await _service.UpdateAsync(id, dto, ct);
            return ok ? NoContent() : NotFound();
        }

        // PATCH: api/products/{id}/availability?isAvailable=true
        [HttpPatch("{id:guid}/availability")]
        public async Task<IActionResult> UpdateAvailability(Guid id, [FromQuery] bool isAvailable, CancellationToken ct = default)
        {
            var ok = await _service.UpdateAvailabilityAsync(id, isAvailable, ct);
            return ok ? NoContent() : NotFound();
        }

        // DELETE: api/products/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
        {
            var ok = await _service.DeleteAsync(id, ct);
            return ok ? NoContent() : NotFound();
        }

        [HttpPost("{id:guid}/image")]
        public async Task<IActionResult> UploadImage(Guid id, IFormFile file,
            [FromServices] IImageStorage storage,
            [FromServices] IProductRepository productRepo,
            CancellationToken ct)
        {
            if (file == null || file.Length == 0) return BadRequest("Empty file.");
            var allowed = new[] { "image/jpeg", "image/png", "image/webp" };
            if (!allowed.Contains(file.ContentType)) return BadRequest("Unsupported content type.");

            // Tamaño máximo: 5MB
            const long maxSize = 5 * 1024 * 1024;
            if (file.Length > maxSize) return BadRequest("File too large (max 5MB).");

            var product = await productRepo.GetByIdAsync(id, ct);
            if (product is null) return NotFound();

            // Nombre único y “carpeta” por producto
            var ext = Path.GetExtension(file.FileName);
            var objectKey = $"products/{id}/{Guid.NewGuid()}{ext}";

            await using var stream = file.OpenReadStream();
            var url = await storage.UploadAsync(stream, file.ContentType, objectKey, ct);

            product.SetImageUrl(url);
            await productRepo.UpdateAsync(product, ct);

            return Ok(new { imageUrl = url });
        }
    }
}
