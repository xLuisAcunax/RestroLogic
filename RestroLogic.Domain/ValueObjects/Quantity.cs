namespace RestroLogic.Domain.ValueObjects
{
    public sealed record Quantity
    {
        public int Value { get; }

        private Quantity(int value)
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Quantity must be > 0");
            Value = value;
        }

        public static Quantity From(int value) => new(value);
    }
}
