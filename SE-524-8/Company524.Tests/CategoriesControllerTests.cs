using Company524.API.Controllers;
using Company524.API.Exceptions;
using Company524.API.Models.Category;
using Company524.API.Models.Common;
using Company524.API.Models.Product;
using Company524.API.Service.Contracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Company524.Tests
{
    public class CategoriesControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock = new();
        private readonly Mock<ICategoryService> _categoryServiceMock = new();
        private readonly CategoriesController _sut;

        private static readonly Guid ValidId = Guid.NewGuid();
        private static readonly PagedRequestDto DefaultPaging = new() { PageNumber = 1, PageSize = 10 };

        public CategoriesControllerTests()
        {
            _sut = new CategoriesController(_productServiceMock.Object, _categoryServiceMock.Object);
        }

        [Fact]
        public async Task GetCategories_WhenCalled_Returns200()
        {
            var pagedResponse = new PagedResponseDto<CategoryForGettingDto>
            {
                Items = [new CategoryForGettingDto { Id = ValidId, CategoryName = "Electronics" }],
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            _categoryServiceMock.Setup(s => s.GetAllCategoriesAsync(DefaultPaging)).ReturnsAsync(pagedResponse);

            var actionResult = await _sut.GetCategories(DefaultPaging);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetCategory_WhenExists_Returns200WithDto()
        {
            var dto = new CategoryForGettingDto { Id = ValidId, CategoryName = "Electronics" };
            _categoryServiceMock.Setup(s => s.GetCategoryByIdAsync(ValidId)).ReturnsAsync(dto);

            var actionResult = await _sut.GetCategory(ValidId);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetCategory_WhenNotFound_ThrowsNotFoundException()
        {
            _categoryServiceMock
                .Setup(s => s.GetCategoryByIdAsync(ValidId))
                .ThrowsAsync(new NotFoundException("Category not found"));

            var act = () => _sut.GetCategory(ValidId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task CreateCategory_WhenValid_Returns201()
        {
            var model = new CategoryForCreatingDto { CategoryName = "Sports" };
            _categoryServiceMock.Setup(s => s.CreateCategoryAsync(model)).ReturnsAsync(ValidId);

            var actionResult = await _sut.CreateCategory(model);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task DeleteCategory_WhenExists_Returns200()
        {
            _categoryServiceMock.Setup(s => s.DeleteCategoryAsync(ValidId)).Returns(Task.CompletedTask);

            var actionResult = await _sut.DeleteCategory(ValidId);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task UpdateCategory_WhenValid_Returns200()
        {
            var model = new CategoryForUpdatingDto { Id = ValidId, CategoryName = "Updated" };
            var dto = new CategoryForGettingDto { Id = ValidId, CategoryName = "Updated" };
            _categoryServiceMock.Setup(s => s.UpdateCategoryAsync(model)).ReturnsAsync(dto);

            var actionResult = await _sut.UpdateCategory(model);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetCategoryProducts_WhenCategoryHasProducts_Returns200()
        {
            var pagedResponse = new PagedResponseDto<ProductListForGettingDto>
            {
                Items = [new ProductListForGettingDto { Id = ValidId, ProductName = "Laptop" }],
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            _productServiceMock
                .Setup(s => s.GetAllProductsOfCategoryAsync(ValidId, DefaultPaging))
                .ReturnsAsync(pagedResponse);

            var actionResult = await _sut.GetCategoryProducts(ValidId, DefaultPaging);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
