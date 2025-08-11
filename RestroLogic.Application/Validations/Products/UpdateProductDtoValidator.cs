using FluentValidation;
using RestroLogic.Application.Dtos.Products;

namespace RestroLogic.Application.Validations.Products
{
    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(120);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.ImageUrl)
                .Must(BeValidUrlOrNull)
                .WithMessage("ImageUrl must be a valid absolute URL.");
        }

        private static bool BeValidUrlOrNull(string? url)
            => string.IsNullOrWhiteSpace(url) || Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}
