namespace RestroLogic.Domain.ValueObjects
{
    public sealed record Money(decimal Amount, string Currency = "COP")
    {
        public static Money operator +(Money a, Money b)
        {
            if (a.Currency != b.Currency)
                throw new InvalidOperationException("Currency mismatch");
            return new Money(a.Amount + b.Amount, a.Currency);
        }
    }
}
