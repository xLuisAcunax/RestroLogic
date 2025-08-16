using FluentValidation;
using RestroLogic.Application.Orders.Commands;

namespace RestroLogic.Application.Common.Validations
{
    public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.TableId).NotEmpty();
        }
    }
}
