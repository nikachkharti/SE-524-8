using Company524.API.Models.Category;

namespace Company524.API.Models.Product
{
    public record ProductForGettingDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public CategoryForGettingDto Category { get; set; }
    }
}
