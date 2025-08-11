using FluentValidation;
using RestroLogic.Application.Dtos.Orders;
using static RestroLogic.Application.Validations.Orders.CreateOrderDtoValidator;

namespace RestroLogic.Application.Validations.Orders
{
    public class UpdateOrderDtoValidator : AbstractValidator<UpdateOrderDto>
    {
        public UpdateOrderDtoValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty();
            RuleFor(x => x.Items)
                .NotNull()
                .Must(i => i.Count > 0).WithMessage("At least one item is required.");

            RuleForEach(x => x.Items)
                .SetValidator(new OrderItemDtoValidator());
        }

    }
}
