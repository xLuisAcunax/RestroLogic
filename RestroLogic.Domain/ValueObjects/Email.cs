namespace RestroLogic.Domain.ValueObjects
{
    public sealed record Email(string Value)
    {
        public static Email Create(string value) 
        {
            if (string.IsNullOrEmpty(value) || !value.Contains("@"))
                throw new ArgumentException("Email inválido", nameof(value));
            return new Email(value);
        } 
    }
}
