using FluentValidation;
using RestroLogic.Application.Dtos.Products;

namespace RestroLogic.Application.Validations.Products
{
    public class ProductQueryParamsValidator : AbstractValidator<ProductQueryParams>
    {
        private static readonly string[] AllowedSortBy = { "name", "price", "id" };
        private static readonly string[] AllowedSortDir = { "asc", "desc" };

        public ProductQueryParamsValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);

            RuleFor(x => x.SortBy)
                .Must(s => s == null || AllowedSortBy.Contains(s.ToLower()))
                .WithMessage($"SortBy must be one of: {string.Join(", ", AllowedSortBy)}");

            RuleFor(x => x.SortDir)
                .Must(s => s == null || AllowedSortDir.Contains(s.ToLower()))
                .WithMessage($"SortDir must be one of: {string.Join(", ", AllowedSortDir)}");
        }
    }
}
