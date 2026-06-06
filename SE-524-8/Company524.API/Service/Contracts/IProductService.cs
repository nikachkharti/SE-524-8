using Company524.API.Models.Product;

namespace Company524.API.Service.Contracts
{
    public interface IProductService
    {
        Task<int> CreateNewProductAsync(ProductForCreatingDto request);
        Task<IEnumerable<ProductListForGettingDto>> GetAllProductsAsync();
        Task<ProductForGettingDto> GetProductAsync(Guid productId);
        Task<int> DeleteProductAsync(Guid productId);
        Task<int> UpdateProductAsync(ProductForUpdatingDto request);
    }
}
