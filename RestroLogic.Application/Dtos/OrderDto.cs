using RestroLogic.Domain.ValueObjects;

namespace RestroLogic.Application.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public Money Total { get; set; } = new Money(0);
        public IEnumerable<OrderItemDto> Items { get; set; } = [];
    }
}
