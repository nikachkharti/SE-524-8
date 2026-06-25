using Company524.API.Entities;
using Company524.API.Exceptions;
using Company524.API.Models.Category;
using Company524.API.Models.Common;
using Company524.API.Repository.Contracts;
using Company524.API.Service;
using FluentAssertions;
using MapsterMapper;
using Moq;

namespace Company524.Tests
{
    /// <summary>
    /// Unit tests for CategoryService.
    ///
    /// KEY CONCEPT — Mocking vs Real implementations:
    /// We use Mock&lt;ICategoryRepository&gt; instead of a real DB because:
    ///   1. Tests run in milliseconds (no network/disk)
    ///   2. We can force any scenario — e.g. "return null" to simulate missing data
    ///   3. We can verify HOW the dependency was called (Times.Once, Times.Never)
    ///
    /// We use Moq (the most popular .NET mocking library):
    ///   Setup()  — tells the mock what to return when called
    ///   Verify() — asserts the mock was called the expected number of times
    /// </summary>
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _categoryRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly CategoryService _sut;

        private static readonly Guid ValidId = Guid.NewGuid();

        public CategoryServiceTests()
        {
            _sut = new CategoryService(_categoryRepoMock.Object, _mapperMock.Object);
        }


        // -----------------------------------------------------------------------
        // CreateCategoryAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task CreateCategoryAsync_WhenModelIsNull_ThrowsBadRequestException()
        {
            var act = () => _sut.CreateCategoryAsync(null!);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*required*");
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenNameExceeds50Chars_ThrowsBadRequestException()
        {
            var model = new CategoryForCreatingDto { CategoryName = new string('A', 51) };

            var act = () => _sut.CreateCategoryAsync(model);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*exceed 50*");
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenNameIsExactly50Chars_DoesNotThrow()
        {
            // Boundary value test: exactly 50 chars must be accepted (≤ 50 is valid)
            var model = new CategoryForCreatingDto { CategoryName = new string('A', 50) };
            var category = new Category { Id = ValidId, CategoryName = model.CategoryName };

            _mapperMock.Setup(m => m.Map<Category>(model)).Returns(category);
            _categoryRepoMock.Setup(r => r.AddAsync(category)).Returns(Task.CompletedTask);
            _categoryRepoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var act = () => _sut.CreateCategoryAsync(model);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenValid_SavesAndReturnsId()
        {
            var model = new CategoryForCreatingDto { CategoryName = "Electronics" };
            var category = new Category { Id = ValidId, CategoryName = "Electronics" };

            _mapperMock.Setup(m => m.Map<Category>(model)).Returns(category);
            _categoryRepoMock.Setup(r => r.AddAsync(category)).Returns(Task.CompletedTask);
            _categoryRepoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _sut.CreateCategoryAsync(model);

            result.Should().Be(ValidId);
            _categoryRepoMock.Verify(r => r.AddAsync(category), Times.Once);
            _categoryRepoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // -----------------------------------------------------------------------
        // GetCategoryByIdAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetCategoryByIdAsync_WhenIdIsEmpty_ThrowsBadRequestException()
        {
            var act = () => _sut.GetCategoryByIdAsync(Guid.Empty);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*id is required*");
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WhenCategoryNotFound_ThrowsNotFoundException()
        {
            _categoryRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Category>, IQueryable<Category>>>()))
                .ReturnsAsync((Category?)null);

            var act = () => _sut.GetCategoryByIdAsync(ValidId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"*{ValidId}*");
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WhenCategoryExists_ReturnsMappedDto()
        {
            var category = new Category { Id = ValidId, CategoryName = "Books" };
            var dto = new CategoryForGettingDto { Id = ValidId, CategoryName = "Books" };

            _categoryRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Category>, IQueryable<Category>>>()))
                .ReturnsAsync(category);

            _mapperMock.Setup(m => m.Map<CategoryForGettingDto>(category)).Returns(dto);

            var result = await _sut.GetCategoryByIdAsync(ValidId);

            result.Should().NotBeNull();
            result.CategoryName.Should().Be("Books");
        }

        // -----------------------------------------------------------------------
        // GetAllCategoriesAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetAllCategoriesAsync_WhenNoCategoriesExist_ReturnsEmptyResponse()
        {
            _categoryRepoMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, object>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(), It.IsAny<bool>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, object>>[]>()))
                .ReturnsAsync((Enumerable.Empty<Category>(), 0));

            var result = await _sut.GetAllCategoriesAsync(new PagedRequestDto());

            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task GetAllCategoriesAsync_WhenCategoriesExist_ReturnsMappedPagedResponse()
        {
            var categories = new List<Category>
        {
            new() { Id = Guid.NewGuid(), CategoryName = "Electronics" },
            new() { Id = Guid.NewGuid(), CategoryName = "Books" }
        };

            var dtos = categories.Select(c => new CategoryForGettingDto { Id = c.Id, CategoryName = c.CategoryName }).ToList();

            _categoryRepoMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, object>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(), It.IsAny<bool>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, object>>[]>()))
                .ReturnsAsync((categories.AsEnumerable(), 2));

            _mapperMock
                .Setup(m => m.Map<IEnumerable<CategoryForGettingDto>>(categories))
                .Returns(dtos);

            var parameters = new PagedRequestDto { PageNumber = 1, PageSize = 10 };
            var result = await _sut.GetAllCategoriesAsync(parameters);

            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        // -----------------------------------------------------------------------
        // DeleteCategoryAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task DeleteCategoryAsync_WhenIdIsEmpty_ThrowsBadRequestException()
        {
            var act = () => _sut.DeleteCategoryAsync(Guid.Empty);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task DeleteCategoryAsync_WhenCategoryNotFound_ThrowsNotFoundException()
        {
            _categoryRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Category>, IQueryable<Category>>>()))
                .ReturnsAsync((Category?)null);

            var act = () => _sut.DeleteCategoryAsync(ValidId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*not found*");
        }

        [Fact]
        public async Task DeleteCategoryAsync_WhenCategoryExists_RemovesAndSaves()
        {
            var category = new Category { Id = ValidId, CategoryName = "Sports" };

            _categoryRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Category>, IQueryable<Category>>>()))
                .ReturnsAsync(category);

            _categoryRepoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _sut.DeleteCategoryAsync(ValidId);

            _categoryRepoMock.Verify(r => r.Remove(category), Times.Once);
            _categoryRepoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // -----------------------------------------------------------------------
        // UpdateCategoryAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task UpdateCategoryAsync_WhenModelIsNull_ThrowsBadRequestException()
        {
            var act = () => _sut.UpdateCategoryAsync(null!);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenIdIsEmpty_ThrowsBadRequestException()
        {
            var model = new CategoryForUpdatingDto { Id = Guid.Empty, CategoryName = "Valid" };

            var act = () => _sut.UpdateCategoryAsync(model);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*id is required*");
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenNameExceeds50Chars_ThrowsBadRequestException()
        {
            var model = new CategoryForUpdatingDto { Id = ValidId, CategoryName = new string('X', 51) };

            var act = () => _sut.UpdateCategoryAsync(model);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*exceed 50*");
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenCategoryNotFound_ThrowsNotFoundException()
        {
            var model = new CategoryForUpdatingDto { Id = ValidId, CategoryName = "Valid" };

            _categoryRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Category>, IQueryable<Category>>>()))
                .ReturnsAsync((Category?)null);

            var act = () => _sut.UpdateCategoryAsync(model);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateCategoryAsync_WhenValid_UpdatesAndReturnsMappedDto()
        {
            var model = new CategoryForUpdatingDto { Id = ValidId, CategoryName = "Updated" };
            var existing = new Category { Id = ValidId, CategoryName = "Old" };
            var dto = new CategoryForGettingDto { Id = ValidId, CategoryName = "Updated" };

            _categoryRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Category>, IQueryable<Category>>>()))
                .ReturnsAsync(existing);

            _categoryRepoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<CategoryForGettingDto>(existing)).Returns(dto);

            var result = await _sut.UpdateCategoryAsync(model);

            result.CategoryName.Should().Be("Updated");
            _categoryRepoMock.Verify(r => r.Update(existing), Times.Once);
            _categoryRepoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
