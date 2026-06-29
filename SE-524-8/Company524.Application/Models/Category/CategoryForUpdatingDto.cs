namespace Company524.Application.Models.Category
{
    public record CategoryForUpdatingDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
    }
}
