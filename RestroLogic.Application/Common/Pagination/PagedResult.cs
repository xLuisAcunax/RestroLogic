namespace RestroLogic.Application.Common.Pagination
{
    public class PagedResult<T>
    {
        public required IEnumerable<T> Items { get; init; }
        public int Total { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
    }
}
