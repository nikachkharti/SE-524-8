namespace Company524.Application.Models.External
{
    public record ExternalProductDto
    {
        public int ExternalId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
    }
}
