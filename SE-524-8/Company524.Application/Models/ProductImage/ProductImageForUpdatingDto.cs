using Microsoft.AspNetCore.Http;

namespace Company524.Application.Models.ProductImage
{
    public record ProductImageForUpdatingDto
    {
        public Guid? ExistingImageId { get; set; }
        public IFormFile File { get; set; }
    }
}
