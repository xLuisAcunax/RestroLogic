using FluentValidation;

namespace RestroLogic.Application.Commands.Sales.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.TableNumber)
            .GreaterThan(0).WithMessage("Table number must be greater than 0");
        }
    }
}
