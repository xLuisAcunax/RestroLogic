using RestroLogic.Domain.ValueObjects;

namespace RestroLogic.Domain.Entities
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal SubTotal => UnitPrice * Quantity;

        protected OrderItem() { }

        public OrderItem(Product product, int quantity)
        {
            ProductId = product.Id;
            ProductName = product.Name;
            Quantity = quantity;
            UnitPrice = product.Price;
        }

        public void IncreaseQuantity(int quantity) => Quantity += quantity;
    }
}
