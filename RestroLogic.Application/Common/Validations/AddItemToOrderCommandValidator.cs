using FluentValidation;
using RestroLogic.Application.Orders.Commands;

namespace RestroLogic.Application.Common.Validations
{
    public sealed class AddItemToOrderCommandValidator : AbstractValidator<AddItemToOrderCommand>
    {
        public AddItemToOrderCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Qty).GreaterThanOrEqualTo(1);
        }
    }
}
