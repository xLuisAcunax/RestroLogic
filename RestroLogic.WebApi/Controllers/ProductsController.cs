using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestroLogic.Application.Products.Commands;
using RestroLogic.Application.Products.Dtos;
using RestroLogic.Application.Products.Queries;

namespace RestroLogic.WebApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public sealed class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateProductCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetActive), new { id }, id);
        }

        [HttpPatch("{id:guid}/toggle")]
        public async Task<IActionResult> Toggle(Guid id)
        {
            await _mediator.Send(new ToggleProductStatusCommand(id));
            return NoContent();
        }

        [HttpGet("active")]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetActive()
        {
            var list = await _mediator.Send(new GetActiveProductsQuery());
            return Ok(list);
        }
    }
}
