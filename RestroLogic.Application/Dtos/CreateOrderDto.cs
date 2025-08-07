using System.ComponentModel.DataAnnotations;

namespace RestroLogic.Application.Dtos
{
    public class CreateOrderDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        [MinLength(1)]
        public List<OrderItemDto> Items { get; set; } = [];
    }
}
