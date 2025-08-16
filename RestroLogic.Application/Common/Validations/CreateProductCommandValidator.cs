using FluentValidation;
using RestroLogic.Application.Products.Commands;

namespace RestroLogic.Application.Common.Validations
{
    public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(150);
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}
