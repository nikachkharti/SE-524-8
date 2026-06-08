namespace Company524.API.Models.Common
{
    public record PagedRequestDto
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; }
        public bool Ascending { get; set; } = true;
    }
}
