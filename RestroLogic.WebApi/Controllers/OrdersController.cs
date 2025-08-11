using Microsoft.AspNetCore.Mvc;
using RestroLogic.Application.Common.Pagination;
using RestroLogic.Application.Dtos.Orders;
using RestroLogic.Application.Interfaces;

namespace RestroLogic.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        // GET: api/orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        // POST: api/orders
        [HttpPost]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] CreateOrderDto dto)
        {
            var id = await _orderService.CreateAsync(dto);
            // Devuelve 201 Created con ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetOrder), new { id }, id);
        }

        // PUT: api/orders/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(Guid id, [FromBody] UpdateOrderDto dto)
        {
            var updated = await _orderService.UpdateAsync(id, dto);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent(); // 204 No content
        }

        // DELETE: api/orders/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var deleted = await _orderService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<OrderDto>>> Search([FromQuery] OrderQueryParams qp, CancellationToken ct)
        {
            var result = await _orderService.SearchAsync(qp, ct);
            return Ok(result);
        }
    }
}
