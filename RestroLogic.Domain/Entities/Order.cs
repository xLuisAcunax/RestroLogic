using RestroLogic.Domain.Exceptions;

namespace RestroLogic.Domain.Entities
{
    public sealed class Order
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid TableId { get; private set; }
        public DateTime OpenedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; private set; }
        public string Status { get; private set; } = "Open";

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        public decimal Total { get; private set; }

        private Order() { }
        public Order(Guid tableId)
        {
            if (tableId == Guid.Empty) throw new ArgumentException("TableId is required.");
            TableId = tableId;
        }

        public OrderItem AddItem(Product product, int qty)
        {
            if (Status != "Open") throw new DomainException("Order is not open.");
            if (product is null) throw new ArgumentNullException(nameof(product));
            if (!product.IsActive) throw new DomainException("Inactive product cannot be added.");
            if (qty < 1) throw new DomainException("Qty must be >= 1.");

            var item = new OrderItem(this.Id, product.Id, qty, product.Price);
            _items.Add(item);
            RecalcTotal();
            return item;
        }

        public void Close()
        {
            if (!_items.Any()) throw new DomainException("Cannot close an order without items.");
            Status = "Closed";
            ClosedAt = DateTime.UtcNow;
        }

        public void RecalcTotal() => Total = _items.Sum(i => i.LineTotal);
    }
}
