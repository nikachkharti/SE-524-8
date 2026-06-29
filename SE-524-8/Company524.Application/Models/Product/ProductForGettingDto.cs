using Company524.Application.Models.Category;
using Company524.Application.Models.Supplier;

namespace Company524.Application.Models.Product
{
    public record ProductForGettingDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public CategoryForGettingDto Category { get; set; }
        public SupplierForGettingDto Supplier { get; set; }
    }
}
