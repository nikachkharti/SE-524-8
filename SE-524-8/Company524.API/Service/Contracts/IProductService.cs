using Company524.API.Entities;
using Company524.API.Models.Product;

namespace Company524.API.Service.Contracts
{
    public interface IProductService
    {
        Task<int> CreateNewProductAsync(ProductForCreatingDto request);
        Task<IEnumerable<ProductForGettingDto>> GetAllProductsAsync();
        //TODO Product Details...
        Task<int> DeleteProductAsync(Guid productId);
        Task<int> UpdateProductAsync(ProductForUpdatingDto request);
    }
}
