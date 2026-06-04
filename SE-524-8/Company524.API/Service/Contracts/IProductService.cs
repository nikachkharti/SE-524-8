using Company524.API.Models.Product;

namespace Company524.API.Service.Contracts
{
    public interface IProductService
    {
        Task<int> CreateNewProductAsync(ProductForCreatingDto request);
    }
}
