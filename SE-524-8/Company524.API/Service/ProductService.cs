using Company524.API.Entities;
using Company524.API.Models.Category;
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

        public async Task<int> DeleteProductAsync(Guid productId)
        {
            var product = await productRepository.GetAsync(p => p.Id == productId);
            if (product is null)
                return 0; //TODO Throw Excpetion

            productRepository.Remove(product);
            return await productRepository.SaveAsync();
        }

        public async Task<IEnumerable<ProductForGettingDto>> GetAllProductsAsync()
        {
            //TODO Optimize...
            var products = (await productRepository.GetAllAsync(
                    includes: p => p.Category)
                ).Items;

            //Mapping
            return products.Select(p => new ProductForGettingDto()
            {
                Id = p.Id,
                ProductName = p.ProductName,
                Price = p.Price,
                Category = new CategoryForGettingDto()
                {
                    Id = p.Category.Id,
                    CategoryName = p.Category.CategoryName
                }
            });
        }

        public async Task<int> UpdateProductAsync(ProductForUpdatingDto request)
        {
            var product = await productRepository.GetAsync(p => p.Id == request.Id);
            if (product is null)
                return 0; //TODO Throw Exception

            // Update the product properties
            product.ProductName = request.ProductName;
            product.Price = request.Price;
            product.Quantity = request.Quantity;
            product.CategoryId = request.CategoryId;
            product.SupplierId = request.SupplierId;

            productRepository.Update(product);
            return await productRepository.SaveAsync();
        }
    }
}
