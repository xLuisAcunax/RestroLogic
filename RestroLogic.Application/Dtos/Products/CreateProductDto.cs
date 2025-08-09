using System.ComponentModel.DataAnnotations;

namespace RestroLogic.Application.Dtos.Products
{
    public class CreateProductDto
    {
        [Required, StringLength(120)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
    }
}
