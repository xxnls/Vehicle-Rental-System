namespace API.Models
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalItemCount { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }

}
