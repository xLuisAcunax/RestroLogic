using RestroLogic.Domain.ValueObjects;

namespace RestroLogic.Domain.Sales
{
    public class Order
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public int TableNumber { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? ClosedAt { get; private set; }
        public bool IsClosed => ClosedAt is not null;

        private readonly List<OrderItem> _items = new();
        public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

        private Order() { }

        public Order(int tableNumber)
        {
            if (tableNumber <= 0)
                throw new ArgumentException("Table number must be positive", nameof(tableNumber));
            TableNumber = tableNumber;
        }

        public static Order Open(int tableNumber) => new Order(tableNumber);

        public void AddItem(Guid menuItemId, string name, Money price, Quantity qty)
        {
            if (IsClosed)
                throw new InvalidOperationException("Cannot add items to a closed order.");
            var item = new OrderItem(Id, menuItemId, name, price, qty);
            _items.Add(item);
        }

        public Money Total()
        {
            var currency = _items.FirstOrDefault()?.UnitPrice.Currency ?? "COP";
            var sum = _items.Sum(i => i.Subtotal().Amount);
            return Money.From(sum, currency);
        }

        public void Close()
        {
            if (!_items.Any())
                throw new InvalidOperationException("Cannot close an empty order.");
            ClosedAt = DateTime.UtcNow;
        }
    }
}
