namespace RestroLogic.Domain.Entities
{
    public sealed class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public decimal Price { get; private set; }
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private Product() { }
        public Product(string name, string? description, decimal price)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required.");
            if (price <= 0) throw new ArgumentException("Price must be greater than 0.");
            Name = name.Trim();
            Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            Price = price;
        }

        public void ToggleStatus() => IsActive = !IsActive;
    }
}
