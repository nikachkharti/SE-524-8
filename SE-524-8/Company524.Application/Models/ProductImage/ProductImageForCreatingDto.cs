using Microsoft.AspNetCore.Http;

namespace Company524.Application.Models.ProductImage
{
    public record ProductImageForCreatingDto
    {
        public IFormFile File { get; set; }
        public Guid ProductId { get; set; }
    }
}
