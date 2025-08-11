namespace RestroLogic.Application.Dtos.Orders
{
    public class OrderQueryParams
    {
        public Guid? CustomerId { get; set; }
        public DateTime? From { get; set; }      // fecha/hora mínima (UTC o local, define convención)
        public DateTime? To { get; set; }        // fecha/hora máxima
        public string? SortBy { get; set; } = "date"; // date | id
        public string? SortDir { get; set; } = "desc"; // asc | desc
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
