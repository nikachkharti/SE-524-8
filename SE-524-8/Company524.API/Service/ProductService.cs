using Company524.API.Entities;
using Company524.API.Exceptions;
using Company524.API.Models.Product;
using Company524.API.Repository.Contracts;
using Company524.API.Service.Contracts;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

namespace Company524.API.Service
{
    public class ProductService(
        IProductRepository productRepository,
        ICategoryRepository categoryRepository,
        ISupplierRepository supplierRepository,
        IMapper mapper) : IProductService
    {
        public async Task<int> CreateNewProductAsync(ProductForCreatingDto request)
        {
            if (request is null)
                throw new BadRequestException($"Request model is required");

            if (request.ProductName.Length > 100)
                throw new BadRequestException("Product name length can't exceed 100");

            if (request.Price < 0)
                throw new BadRequestException("Product price can't be a negative number");

            if (request.Quantity < 0)
                throw new BadRequestException("Product quantity can't be a negative number");

            if (request.CategoryId == Guid.Empty)
                throw new BadRequestException("Product category id is required");

            if (request.SupplierId == Guid.Empty)
                throw new BadRequestException("Product supplier id is required");

            if (!await CategoryExists(request.CategoryId))
                throw new BadRequestException("Category not found");

            if (!await SupplierExists(request.SupplierId))
                throw new BadRequestException("Supplier not found");

            var newProduct = mapper.Map<Product>(request);
            await productRepository.AddAsync(newProduct);
            return await productRepository.SaveAsync();
        }

        public async Task<int> DeleteProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new BadRequestException("Product id is required");

            var product = await productRepository.GetAsync(p => p.Id == productId);

            if (product is null)
                throw new NotFoundException($"Product with ID {productId} not found.");

            productRepository.Remove(product);
            return await productRepository.SaveAsync();
        }


        //TODO : Add Pagination
        public async Task<IEnumerable<ProductListForGettingDto>> GetAllProductsAsync()
        {
            var products = await productRepository.GetAllAsync();

            if (products.Items.Count() == 0)
                return Enumerable.Empty<ProductListForGettingDto>();

            var result = mapper.Map<IEnumerable<ProductListForGettingDto>>(products.Items);
            return result;
        }

        public async Task<ProductForGettingDto> GetProductAsync(Guid productId)
        {
            if (productId == Guid.Empty)
                throw new BadRequestException("Product id is required");

            var product = await productRepository.GetAsync(
                    filter: p => p.Id == productId,
                    include: p => p
                            .Include(p => p.Category)
                            .Include(p => p.Supplier)
            );

            if (product is null)
                throw new NotFoundException($"Product with id: {productId} not found");

            return mapper.Map<ProductForGettingDto>(product);
        }

        public async Task<int> UpdateProductAsync(ProductForUpdatingDto request)
        {
            if (request is null)
                throw new BadRequestException($"Request model is required");

            if (request.Id == Guid.Empty)
                throw new BadRequestException("Product id is required");

            if (request.ProductName.Length > 100)
                throw new BadRequestException("Product name length can't exceed 100");

            if (request.Price < 0)
                throw new BadRequestException("Product price can't be a negative number");

            if (request.Quantity < 0)
                throw new BadRequestException("Product quantity can't be a negative number");

            if (request.CategoryId == Guid.Empty)
                throw new BadRequestException("Product category id is required");

            if (request.SupplierId == Guid.Empty)
                throw new BadRequestException("Product supplier id is required");

            if (!await CategoryExists(request.CategoryId))
                throw new BadRequestException("Category not found");

            if (!await SupplierExists(request.SupplierId))
                throw new BadRequestException("Supplier not found");

            var product = await productRepository.GetAsync(p => p.Id == request.Id);

            if (product is null)
                throw new NotFoundException($"Product with id: {request.Id} not found");

            mapper.Map(request, product);
            productRepository.Update(product);
            return await productRepository.SaveAsync();
        }



        private async Task<bool> CategoryExists(Guid categoryId)
            => await categoryRepository.ExistsAsync(c => c.Id == categoryId);

        private async Task<bool> SupplierExists(Guid supplierId)
            => await supplierRepository.ExistsAsync(s => s.Id == supplierId);

    }
}
