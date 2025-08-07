using RestroLogic.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace RestroLogic.Application.Dtos
{
    public class OrderItemDto
    {
        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;

        public string ProductDescription { get; set; }

        [Range(0, int.MaxValue)]
        public Money UnitPrice { get; set; } = new Money(0);

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
