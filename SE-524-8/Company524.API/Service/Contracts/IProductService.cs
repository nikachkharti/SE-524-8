using Company524.API.Models.Common;
using Company524.API.Models.Product;

namespace Company524.API.Service.Contracts
{
    public interface IProductService
    {
        Task<int> CreateNewProductAsync(ProductForCreatingDto request);
        Task<PagedResponseDto<ProductListForGettingDto>> GetAllProductsAsync(PagedRequestDto parameters);
        Task<PagedResponseDto<ProductListForGettingDto>> GetAllProductsOfSupplierAsync(
            Guid supplierId,
            PagedRequestDto parameters
        );
        Task<PagedResponseDto<ProductListForGettingDto>> GetAllProductsOfCategoryAsync(
            Guid categoryId,
            PagedRequestDto parameters
        );
        Task<ProductForGettingDto> GetProductAsync(Guid productId);
        Task<int> DeleteProductAsync(Guid productId);
        Task<int> UpdateProductAsync(ProductForUpdatingDto request);
    }
}
