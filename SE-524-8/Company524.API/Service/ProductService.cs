using Company524.API.Entities;
using Company524.API.Models.Category;
using Company524.API.Models.Product;
using Company524.API.Repository.Contracts;
using Company524.API.Service.Contracts;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<ProductListForGettingDto>> GetAllProductsAsync()
        {
            var products = await productRepository.GetAllAsync();

            var result = mapper.Map<IEnumerable<ProductListForGettingDto>>(products.Items);
            return result;
        }

        public async Task<ProductForGettingDto> GetProductAsync(Guid productId)
        {
            var product = await productRepository.GetAsync(
                    filter: p => p.Id == productId,
                    include: p => p
                            .Include(p => p.Category)
                            .Include(p => p.Supplier)
            );


            return mapper.Map<ProductForGettingDto>(product);
        }

        public async Task<int> UpdateProductAsync(ProductForUpdatingDto request)
        {
            var product = await productRepository.GetAsync(p => p.Id == request.Id);
            if (product is null)
                return 0;

            mapper.Map(request, product);
            productRepository.Update(product);
            return await productRepository.SaveAsync();
        }
    }
}
