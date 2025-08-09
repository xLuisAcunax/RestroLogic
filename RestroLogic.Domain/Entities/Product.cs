namespace RestroLogic.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public string Name { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public decimal Price { get; private set; }
        public bool IsAvailable { get; private set; } = true;
        public string? ImageUrl { get; private set; } = null;

        protected Product() { } 

        public Product(string name, string description, decimal price)
        {
            Rename(name);
            ChangeDescription(description);
            ChangePrice(price);
            IsAvailable = true;
        }

        // --- Comportamiento de dominio ---
        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));
            if (name.Length > 120)
                throw new ArgumentException("Name max length is 120.", nameof(name));

            Name = name.Trim();
        }

        public void ChangeDescription(string? description)
        {
            description ??= string.Empty;
            if (description.Length > 500)
                throw new ArgumentException("Description max length is 500.", nameof(description));

            Description = description.Trim();
        }

        public void ChangePrice(decimal price)
        {
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.", nameof(price));

            Price = decimal.Round(price, 2, MidpointRounding.AwayFromZero);
        }

        public void SetImageUrl(string? imageUrl)
        {
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out _))
                    throw new ArgumentException("Invalid image URL.", nameof(imageUrl));

                ImageUrl = imageUrl.Trim();
            }
            else
            {
                ImageUrl = null;
            }
        }

        public void MarkAsUnavailable() => IsAvailable = false;
        public void MarkAsAvailable() => IsAvailable = true;
    }
}
