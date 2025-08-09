using System.ComponentModel.DataAnnotations;

namespace RestroLogic.Application.Dtos.Orders
{
    public class UpdateOrderDto
    {
        [Required]
        public Guid CustomerId {  get; set; }

        [MinLength(1)]
        public List<OrderItemDto> Items { get; set; } = [];
    }
}
