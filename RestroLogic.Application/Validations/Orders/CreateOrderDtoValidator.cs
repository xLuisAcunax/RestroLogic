using FluentValidation;
using RestroLogic.Application.Dtos.Orders;

namespace RestroLogic.Application.Validations.Orders
{
    public class CreateOrderDtoValidator : AbstractValidator<CreateOrderDto>
    {
        public CreateOrderDtoValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.Items)
                .NotNull()
                .Must(i => i.Count > 0).WithMessage("At least one item is required.");

            RuleForEach(x => x.Items)
                .SetValidator(new OrderItemDtoValidator());
        }

        public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
        {
            public OrderItemDtoValidator()
            {
                RuleFor(x => x.ProductId).NotEmpty();
                RuleFor(x => x.ProductName).NotEmpty().MaximumLength(200);
                RuleFor(x => x.UnitPrice).GreaterThanOrEqualTo(0);
                RuleFor(x => x.Quantity).GreaterThan(0);
            }
        }
    }
}
