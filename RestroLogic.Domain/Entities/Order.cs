using RestroLogic.Domain.ValueObjects;

namespace RestroLogic.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid CustomerId { get; private set; }
        private List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        protected Order() { }

        public Order(Guid customerId)
        {
            CustomerId = customerId;
        }

        public void AddItem(Product product, int quantity)
        {
            var existing = _items.SingleOrDefault(i => i.ProductId == product.Id);
            if (existing is null)
                _items.Add(new OrderItem(product, quantity));
            else
                existing.IncreaseQuantity(quantity);
        }

        public decimal GetTotal()
        {
            var total = 0M;
            foreach (var item in _items)
                total += item.SubTotal;
            return total;
        }
    }
}
