using RestroLogic.Domain.ValueObjects;

namespace RestroLogic.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public bool IsAvaiable { get; private set; } = true;

        protected Product() { }

        public Product(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        public void MarkAsUnavailable() => IsAvaiable = false;
        public void MaskAsAvailable() => IsAvaiable = true;
    }
}
