using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestroLogic.Application.Orders.Commands;
using RestroLogic.Application.Orders.Dtos;
using RestroLogic.Application.Orders.Queries;

namespace RestroLogic.WebApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public sealed class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public OrdersController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateOrderCommand cmd)
        {
            var id = await _mediator.Send(cmd);
            return CreatedAtAction(nameof(GetByTable), new { tableId = cmd.TableId }, id);
        }

        [HttpPost("{orderId:guid}/items")]
        public async Task<ActionResult<Guid>> AddItem(Guid orderId, [FromBody] AddItemToOrderCommand body)
        {
            if (orderId != body.OrderId) return BadRequest("Route id and body id mismatch.");
            var itemId = await _mediator.Send(body);
            return Ok(itemId);
        }

        [HttpGet("by-table/{tableId:guid}")]
        public async Task<ActionResult<OrderSummaryDto>> GetByTable(Guid tableId)
        {
            var dto = await _mediator.Send(new GetOrdersByTableQuery(tableId));
            return Ok(dto);
        }

        [HttpPost("{orderId:guid}/close")]
        public async Task<IActionResult> Close(Guid orderId)
        {
            await _mediator.Send(new CloseOrderCommand(orderId));
            return NoContent();
        }
    }
}
