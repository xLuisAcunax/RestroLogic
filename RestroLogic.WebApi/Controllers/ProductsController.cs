using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestroLogic.Application.Dtos.Products;
using RestroLogic.Application.Interfaces;

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
    }
}
