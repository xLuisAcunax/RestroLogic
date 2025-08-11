namespace RestroLogic.Application.Dtos.Products
{
    public class ProductQueryParams
    {
        public string? Search { get; set; }
        public bool? OnlyAvailable { get; set; }
        public string? SortBy { get; set; } = "name";   // name | price | id
        public string? SortDir { get; set; } = "asc";   // asc | desc
        public int Page { get; set; } = 1;              // 1-based
        public int PageSize { get; set; } = 20;         // 10/20/50
    }
}
