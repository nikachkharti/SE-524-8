namespace Company524.Application.Models.Category
{
    public record CategoryForGettingDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
    }
}
