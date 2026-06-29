using Company524.API.Controllers;
using Company524.Application.Exceptions;
using Company524.Application.Models.Common;
using Company524.Application.Models.Product;
using Company524.Application.Contracts.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Company524.Tests
{
    /// <summary>
    /// Unit tests for Controllers.
    ///
    /// WHAT DO WE TEST IN CONTROLLERS?
    /// Controllers are thin by design — they should only:
    ///   1. Call the service
    ///   2. Wrap the result in a CommonResponse
    ///   3. Return the correct HTTP status code
    ///
    /// We do NOT re-test business logic here (that belongs in service tests).
    /// We mock the service and verify the controller's routing/wrapping behavior.
    ///
    /// CONTROLLER TESTING APPROACH:
    /// We instantiate controllers directly (not through HttpClient) for speed.
    /// For full HTTP pipeline tests (middleware, routing, auth), use
    /// WebApplicationFactory — but that's an integration test, not a unit test.
    /// </summary>
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock = new();
        private readonly ProductsController _sut;

        private static readonly Guid ValidId = Guid.NewGuid();
        private static readonly PagedRequestDto DefaultPaging = new() { PageNumber = 1, PageSize = 10 };

        public ProductsControllerTests()
        {
            _sut = new ProductsController(_productServiceMock.Object);
        }

        // -----------------------------------------------------------------------
        // CreateProduct
        // -----------------------------------------------------------------------

        [Fact]
        public async Task CreateProduct_WhenServiceSucceeds_Returns201WithResult()
        {
            var request = new ProductForCreatingDto { ProductName = "Laptop", Price = 999m, Quantity = 5, CategoryId = ValidId, SupplierId = ValidId };
            _productServiceMock.Setup(s => s.CreateNewProductAsync(request)).ReturnsAsync(1);

            var actionResult = await _sut.CreateProduct(request);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task CreateProduct_WhenServiceThrowsBadRequest_ExceptionPropagates()
        {
            var request = new ProductForCreatingDto();
            _productServiceMock
                .Setup(s => s.CreateNewProductAsync(request))
                .ThrowsAsync(new BadRequestException("Request model is required"));

            // Controllers don't catch exceptions — the middleware does.
            // Here we verify the exception propagates correctly.
            var act = () => _sut.CreateProduct(request);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*required*");
        }

        // -----------------------------------------------------------------------
        // GetAllProducts
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetAllProducts_WhenCalled_Returns200WithPagedResult()
        {
            var pagedResponse = new PagedResponseDto<ProductListForGettingDto>
            {
                Items = [new ProductListForGettingDto { Id = ValidId, ProductName = "Laptop", Price = 999m }],
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            _productServiceMock.Setup(s => s.GetAllProductsAsync(DefaultPaging)).ReturnsAsync(pagedResponse);

            var actionResult = await _sut.GetAllProducts(DefaultPaging);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        // -----------------------------------------------------------------------
        // GetProduct
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetProduct_WhenProductExists_Returns200WithDto()
        {
            var dto = new ProductForGettingDto { Id = ValidId, ProductName = "Laptop", Price = 999m };
            _productServiceMock.Setup(s => s.GetProductAsync(ValidId)).ReturnsAsync(dto);

            var actionResult = await _sut.GetProduct(ValidId);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetProduct_WhenProductNotFound_ThrowsNotFoundException()
        {
            _productServiceMock
                .Setup(s => s.GetProductAsync(ValidId))
                .ThrowsAsync(new NotFoundException("Product not found"));

            var act = () => _sut.GetProduct(ValidId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        // -----------------------------------------------------------------------
        // DeleteProduct
        // -----------------------------------------------------------------------

        [Fact]
        public async Task DeleteProduct_WhenProductExists_Returns204()
        {
            _productServiceMock.Setup(s => s.DeleteProductAsync(ValidId)).ReturnsAsync(1);

            var actionResult = await _sut.DeleteProduct(ValidId);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        // -----------------------------------------------------------------------
        // UpdateProduct
        // -----------------------------------------------------------------------

        [Fact]
        public async Task UpdateProduct_WhenProductExists_Returns200()
        {
            var request = new ProductForUpdatingDto { Id = ValidId, ProductName = "Updated", Price = 500m, Quantity = 1, CategoryId = ValidId, SupplierId = ValidId };
            _productServiceMock.Setup(s => s.UpdateProductAsync(request)).ReturnsAsync(1);

            var actionResult = await _sut.UpdateProduct(request);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
