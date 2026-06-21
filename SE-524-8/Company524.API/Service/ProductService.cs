using Company524.API.Entities;
using Company524.API.Exceptions;
using Company524.API.Models.Common;
using Company524.API.Models.Product;
using Company524.API.Repository.Contracts;
using Company524.API.Service.Contracts;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
        public async Task<PagedResponseDto<ProductListForGettingDto>> GetAllProductsAsync(PagedRequestDto parameters)
        {
            var products = await productRepository.GetAllAsync(
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize
            );

            return MapToPagedResponse(products, parameters);
        }
        public async Task<PagedResponseDto<ProductListForGettingDto>> GetAllProductsOfCategoryAsync(
            Guid categoryId,
            PagedRequestDto parameters)
        {
            if (categoryId == Guid.Empty)
                throw new BadRequestException("Category id is required");

            var products = await productRepository.GetAllAsync(
                filter: p => p.CategoryId == categoryId,
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize
            );

            return MapToPagedResponse(products, parameters);
        }
        public async Task<PagedResponseDto<ProductListForGettingDto>> GetAllProductsOfSupplierAsync(
            Guid supplierId,
            PagedRequestDto parameters)
        {
            if (supplierId == Guid.Empty)
                throw new BadRequestException("Supplier id is required");

            var products = await productRepository.GetAllAsync(
                filter: p => p.SupplierId == supplierId,
                orderBy: BuildOrderBy(parameters.SortBy),
                ascending: parameters.Ascending,
                pageNumber: parameters.PageNumber,
                pageSize: parameters.PageSize
            );

            return MapToPagedResponse(products, parameters);
        }



        // -----Private helpers-----
        private static Expression<Func<Product, object>> BuildOrderBy(string sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "productname" => p => p.ProductName,
                "price" => p => p.Price,
                "quantity" => p => p.Quantity,
                "categoryId" => p => p.CategoryId,
                "supplierId" => p => p.SupplierId,
                _ => p => p.Id
            };
        }
        private PagedResponseDto<ProductListForGettingDto> MapToPagedResponse(
            (IEnumerable<Product> Items, int TotalCount) products,
            PagedRequestDto parameters)
        {
            return new PagedResponseDto<ProductListForGettingDto>
            {
                Items = products.Items.Any()
                    ? mapper.Map<IEnumerable<ProductListForGettingDto>>(products.Items)
                    : Enumerable.Empty<ProductListForGettingDto>(),
                TotalCount = products.TotalCount,
                PageNumber = parameters.PageNumber,
                PageSize = parameters.PageSize
            };
        }
        private async Task<bool> CategoryExists(Guid categoryId)
            => await categoryRepository.ExistsAsync(c => c.Id == categoryId);
        private async Task<bool> SupplierExists(Guid supplierId)
            => await supplierRepository.ExistsAsync(s => s.Id == supplierId);

    }
}
