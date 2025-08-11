using FluentValidation;
using RestroLogic.Application.Dtos.Orders;

namespace RestroLogic.Application.Validations.Orders
{
    public class OrderQueryParamsValidator : AbstractValidator<OrderQueryParams>
    {
        private static readonly string[] AllowedSortBy = { "date", "id" };
        private static readonly string[] AllowedSortDir = { "asc", "desc" };

        public OrderQueryParamsValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
            RuleFor(x => x.SortBy)
                .Must(s => s == null || AllowedSortBy.Contains(s.ToLower()))
                .WithMessage($"SortBy must be one of: {string.Join(", ", AllowedSortBy)}");
            RuleFor(x => x.SortDir)
                .Must(s => s == null || AllowedSortDir.Contains(s.ToLower()))
                .WithMessage($"SortDir must be one of: {string.Join(", ", AllowedSortDir)}");
            RuleFor(x => x)
                .Must(x => !(x.From.HasValue && x.To.HasValue) || x.From <= x.To)
                .WithMessage("'From' must be <= 'To'.");
        }
    }
}
