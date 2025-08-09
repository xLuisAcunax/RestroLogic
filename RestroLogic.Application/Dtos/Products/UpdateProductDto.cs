using System.ComponentModel.DataAnnotations;

namespace RestroLogic.Application.Dtos.Products
{
    public class UpdateProductDto
    {
        [Required, MaxLength(120)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; }
    }
}
