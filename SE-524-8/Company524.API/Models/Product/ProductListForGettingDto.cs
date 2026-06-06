namespace Company524.API.Models.Product
{
    public record ProductListForGettingDto
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}
