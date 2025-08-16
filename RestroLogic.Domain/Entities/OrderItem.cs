namespace RestroLogic.Domain.Entities
{
    public sealed class OrderItem
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Qty { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal LineTotal { get; private set; }

        private OrderItem() { }
        public OrderItem(Guid orderId, Guid productId, int qty, decimal unitPrice)
        {
            if (orderId == Guid.Empty) throw new ArgumentException("OrderId is required.");
            if (productId == Guid.Empty) throw new ArgumentException("ProductId is required.");
            if (qty < 1) throw new ArgumentException("Qty must be >= 1.");
            if (unitPrice <= 0) throw new ArgumentException("UnitPrice must be > 0.");
            OrderId = orderId;
            ProductId = productId;
            Qty = qty;
            UnitPrice = unitPrice;
            LineTotal = unitPrice * qty;
        }
    }
}
