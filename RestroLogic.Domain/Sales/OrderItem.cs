using RestroLogic.Domain.ValueObjects;

namespace RestroLogic.Domain.Sales
{
    public class OrderItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid OrderId { get; private set; }
        public Guid MenuItemId { get; private set; }
        public string MenuItemName { get; private set; }
        public Money UnitPrice { get; private set; }
        public Quantity Quantity { get; private set; }

        private OrderItem() { }

        internal OrderItem(Guid orderId, Guid menuItemId, string name, Money unitPrice, Quantity quantity)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name required", nameof(name));
            OrderId = orderId;
            MenuItemId = menuItemId;
            MenuItemName = name;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public Money Subtotal() => Money.From(UnitPrice.Amount * Quantity.Value, UnitPrice.Currency);
    }
}
