namespace RestroLogic.Domain.ValueObjects
{
    public sealed record Money
    {
        public decimal Amount { get; }
        public string Currency { get; } = "COP";

        private Money(decimal amount, string currency)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Money cannot be negative.");
            if (string.IsNullOrWhiteSpace(currency))
                throw new ArgumentException("Currency required.", nameof(currency));
            Amount = decimal.Round(amount, 0);
            Currency = currency.ToUpperInvariant();
        }

        public static Money From(decimal ammount, string currency = "COP") => new Money(ammount, currency);

        public static Money operator +(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            return From(a.Amount + b.Amount, a.Currency);
        }

        public static Money operator -(Money a, Money b)
        {
            EnsureSameCurrency(a, b);
            var res = a.Amount - b.Amount;
            if (res < 0)
                throw new InvalidOperationException("Money cannot be negative.");
            return From(res, a.Currency);
        }

        private static void EnsureSameCurrency(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Cannot operate on different currencies.");
        }
    }
}
