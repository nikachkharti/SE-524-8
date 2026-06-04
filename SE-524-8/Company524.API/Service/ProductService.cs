using Company524.API.Entities;
using Company524.API.Models.Category;
using Company524.API.Models.Product;
using Company524.API.Repository.Contracts;
using Company524.API.Service.Contracts;
using MapsterMapper;

namespace Company524.API.Service
{
    //TODO : Add Validations

    public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
    {
        public async Task<int> CreateNewProductAsync(ProductForCreatingDto request)
        {
            var newProduct = mapper.Map<Product>(request);
            await productRepository.AddAsync(newProduct);
            return await productRepository.SaveAsync();
        }

        public async Task<int> DeleteProductAsync(Guid productId)
        {
            var product = await productRepository.GetAsync(p => p.Id == productId);
            if (product is null)
                return 0;

            productRepository.Remove(product);
            return await productRepository.SaveAsync();
        }

        public async Task<IEnumerable<ProductForGettingDto>> GetAllProductsAsync()
        {
            //TODO Optimize...
            var products = (await productRepository.GetAllAsync(
                    includes: p => p.Category)
                ).Items;

            var result = mapper.Map<IEnumerable<ProductForGettingDto>>(products);
            return result;
        }

        public async Task<int> UpdateProductAsync(ProductForUpdatingDto request)
        {
            var product = await productRepository.GetAsync(p => p.Id == request.Id);
            if (product is null)
                return 0;

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
