namespace Company524.Application.Models.ProductImage
{
    public record ProductImageForGettingDto
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public string ImagePublicId { get; set; }
    }
}
