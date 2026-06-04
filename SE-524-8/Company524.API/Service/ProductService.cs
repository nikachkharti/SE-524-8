using Azure.Core;
using Company524.API.Entities;
using Company524.API.Models.Product;
using Company524.API.Repository.Contracts;
using Company524.API.Service.Contracts;

namespace Company524.API.Service
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        public async Task<int> CreateNewProductAsync(ProductForCreatingDto request)
        {
            //Mapping
            await productRepository.AddAsync(new Product()
            {
                ProductName = request.ProductName,
                Price = request.Price,
                Quantity = request.Quantity,
                CategoryId = request.CategoryId,
                SupplierId = request.SupplierId
            });
            return await productRepository.SaveAsync();
        }
    }
}
