namespace Company524.API.Models.Category
{
    public record CategoryForGettingDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; }
    }
}
