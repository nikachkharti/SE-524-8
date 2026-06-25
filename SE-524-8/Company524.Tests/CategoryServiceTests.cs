using Company524.API.Entities;
using Company524.API.Exceptions;
using Company524.API.Models.Category;
using Company524.API.Repository.Contracts;
using Company524.API.Service;
using FluentAssertions;
using MapsterMapper;
using Moq;

namespace Company524.Tests
{
    public class CategoryServiceTests
    {
        //Arrange Act Assert  მომზადება მოქმედება მტკიცება

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
            //ARRANGE
            var act = () => _sut.CreateCategoryAsync(null);

            //ACT ASSERT
            await act.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage("*required*");
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenNameExceeds50Chars_ThrowBadRequestException()
        {
            //ARRANGE
            var model = new CategoryForCreatingDto() { CategoryName = new string('A', 51) };

            //ACT
            var act = () => _sut.CreateCategoryAsync(model);

            //ASSERT
            await act.Should()
                .ThrowAsync<BadRequestException>()
                .WithMessage("*exceed 50*");
        }

        [Fact]
        public async Task CreateCategoryAsync_WhenNameIsExactly50Chars_DoesNotThrow()
        {
            //ARRANGE
            var model = new CategoryForCreatingDto() { CategoryName = new string('A', 50) };
            var category = new Category { Id = ValidId, CategoryName = model.CategoryName };

            _mapperMock.Setup(m => m.Map<Category>(model))
                .Returns(category);

            _categoryRepoMock.Setup(r => r.AddAsync(category))
                .Returns(Task.CompletedTask);

            _categoryRepoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);


            //ACT
            var act = () => _sut.CreateCategoryAsync(model);


            //ASSERT
            await act.Should().NotThrowAsync();
        }


        [Fact]
        public async Task CreateCategoryAsync_WhenValid_SavesAndReturnsId()
        {
            //ARRANGE
            var model = new CategoryForCreatingDto() { CategoryName = "Electronics" };
            var category = new Category { Id = ValidId, CategoryName = "Electronics" };

            _mapperMock.Setup(m => m.Map<Category>(model))
                .Returns(category);

            _categoryRepoMock.Setup(r => r.AddAsync(category))
                .Returns(Task.CompletedTask);

            _categoryRepoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);


            //ACT
            var result = await _sut.CreateCategoryAsync(model);


            //ASSERT
            result.Should().Be(ValidId);
            _categoryRepoMock.Verify(r => r.AddAsync(category), Times.Once);
            _categoryRepoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }


    }
}
