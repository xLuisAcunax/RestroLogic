using FluentValidation;

namespace RestroLogic.Application.Commands.Sales.AddItem
{
    public sealed class AddItemCommandValidator : AbstractValidator<AddItemCommand>
    {
        public AddItemCommandValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.MenuItemId).NotEmpty();
            RuleFor(x => x.MenuItemName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.UnitPrice).GreaterThan(0);
            RuleFor(x => x.Currency).NotEmpty().Length(3);
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
