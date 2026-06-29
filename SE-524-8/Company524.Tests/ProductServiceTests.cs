using Company524.Domain.Entities;
using Company524.Application.Exceptions;
using Company524.Application.Models.Common;
using Company524.Application.Models.Product;
using Company524.Application.Contracts.Persistence;
using Company524.Application.Service;
using FluentAssertions;
using MapsterMapper;
using Moq;

namespace Company524.Tests
{
    /// <summary>
    /// Unit tests for ProductService.
    ///
    /// WHAT IS A UNIT TEST?
    /// A unit test verifies a single "unit" of behavior in complete isolation.
    /// "Isolation" means every external dependency (database, email, other services)
    /// is replaced with a fake (Mock) that you control — so the test is:
    ///   • Fast   — no real I/O
    ///   • Reliable — same result every run
    ///   • Precise  — when it fails, you know exactly which logic broke
    ///
    /// ANATOMY OF A TEST (AAA pattern):
    ///   Arrange — set up inputs and configure mocks
    ///   Act     — call the method under test
    ///   Assert  — verify the outcome
    ///
    /// NAMING CONVENTION used here:
    ///   MethodName_StateUnderTest_ExpectedBehavior
    ///   e.g. CreateNewProductAsync_WhenNullRequest_ThrowsBadRequestException
    /// </summary>
    public class ProductServiceTests
    {
        // -----------------------------------------------------------------------
        // Fields — shared mocks created once per test class instance.
        // xUnit creates a NEW instance of this class for each [Fact], so there
        // is no shared state between tests (each test starts with clean mocks).
        // -----------------------------------------------------------------------
        private readonly Mock<IProductRepository> _productRepoMock = new();
        private readonly Mock<ICategoryRepository> _categoryRepoMock = new();
        private readonly Mock<ISupplierRepository> _supplierRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly ProductService _sut; // System Under Test

        public ProductServiceTests()
        {
            _sut = new ProductService(
                _productRepoMock.Object,
                _categoryRepoMock.Object,
                _supplierRepoMock.Object,
                _mapperMock.Object);
        }

        // -----------------------------------------------------------------------
        // Shared test data helpers
        // -----------------------------------------------------------------------
        private static readonly Guid ValidCategoryId = Guid.NewGuid();
        private static readonly Guid ValidSupplierId = Guid.NewGuid();
        private static readonly Guid ValidProductId = Guid.NewGuid();

        private static ProductForCreatingDto ValidCreateRequest() => new()
        {
            ProductName = "Test Laptop",
            Price = 999.99m,
            Quantity = 10,
            CategoryId = ValidCategoryId,
            SupplierId = ValidSupplierId
        };

        private static Product SampleProduct() => new()
        {
            Id = ValidProductId,
            ProductName = "Test Laptop",
            Price = 999.99m,
            Quantity = 10,
            CategoryId = ValidCategoryId,
            SupplierId = ValidSupplierId
        };

        private static PagedRequestDto DefaultPaging() => new()
        {
            PageNumber = 1,
            PageSize = 10,
            Ascending = true
        };

        // -----------------------------------------------------------------------
        // CreateNewProductAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task CreateNewProductAsync_WhenRequestIsNull_ThrowsBadRequestException()
        {
            // Act
            var act = () => _sut.CreateNewProductAsync(null!);

            // Assert
            // FluentAssertions makes the error message readable and precise.
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*required*");
        }

        [Fact]
        public async Task CreateNewProductAsync_WhenProductNameExceeds100Chars_ThrowsBadRequestException()
        {
            // Arrange — a name that is exactly 101 characters long
            var request = ValidCreateRequest() with { ProductName = new string('A', 101) };

            // Act
            var act = () => _sut.CreateNewProductAsync(request);

            // Assert
            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*exceed 100*");
        }

        [Fact]
        public async Task CreateNewProductAsync_WhenPriceIsNegative_ThrowsBadRequestException()
        {
            var request = ValidCreateRequest() with { Price = -1m };

            var act = () => _sut.CreateNewProductAsync(request);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*negative*");
        }

        [Fact]
        public async Task CreateNewProductAsync_WhenQuantityIsNegative_ThrowsBadRequestException()
        {
            var request = ValidCreateRequest() with { Quantity = -5 };

            var act = () => _sut.CreateNewProductAsync(request);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*negative*");
        }

        [Fact]
        public async Task CreateNewProductAsync_WhenCategoryIdIsEmpty_ThrowsBadRequestException()
        {
            var request = ValidCreateRequest() with { CategoryId = Guid.Empty };

            var act = () => _sut.CreateNewProductAsync(request);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*category id*");
        }

        [Fact]
        public async Task CreateNewProductAsync_WhenSupplierIdIsEmpty_ThrowsBadRequestException()
        {
            var request = ValidCreateRequest() with { SupplierId = Guid.Empty };

            var act = () => _sut.CreateNewProductAsync(request);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*supplier id*");
        }

        [Fact]
        public async Task CreateNewProductAsync_WhenCategoryDoesNotExist_ThrowsBadRequestException()
        {
            // Arrange — category lookup returns false
            _categoryRepoMock
                .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>()))
                .ReturnsAsync(false);

            var act = () => _sut.CreateNewProductAsync(ValidCreateRequest());

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*Category not found*");
        }

        [Fact]
        public async Task CreateNewProductAsync_WhenSupplierDoesNotExist_ThrowsBadRequestException()
        {
            // Arrange — category exists but supplier does not
            _categoryRepoMock
                .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>()))
                .ReturnsAsync(true);

            _supplierRepoMock
                .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>()))
                .ReturnsAsync(false);

            var act = () => _sut.CreateNewProductAsync(ValidCreateRequest());

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*Supplier not found*");
        }

        [Fact]
        public async Task CreateNewProductAsync_WhenRequestIsValid_SavesProductAndReturnsRowCount()
        {
            // Arrange
            var request = ValidCreateRequest();
            var newProduct = SampleProduct();

            _categoryRepoMock
                .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>()))
                .ReturnsAsync(true);

            _supplierRepoMock
                .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>()))
                .ReturnsAsync(true);

            _mapperMock
                .Setup(m => m.Map<Product>(request))
                .Returns(newProduct);

            _productRepoMock
                .Setup(r => r.AddAsync(newProduct))
                .Returns(Task.CompletedTask);

            _productRepoMock
                .Setup(r => r.SaveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _sut.CreateNewProductAsync(request);

            // Assert
            result.Should().Be(1);

            // Verify the repository was actually called — important for write operations
            _productRepoMock.Verify(r => r.AddAsync(newProduct), Times.Once);
            _productRepoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // -----------------------------------------------------------------------
        // GetProductAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetProductAsync_WhenIdIsEmpty_ThrowsBadRequestException()
        {
            var act = () => _sut.GetProductAsync(Guid.Empty);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*id is required*");
        }

        [Fact]
        public async Task GetProductAsync_WhenProductNotFound_ThrowsNotFoundException()
        {
            _productRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
                .ReturnsAsync((Product?)null);

            var act = () => _sut.GetProductAsync(ValidProductId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"*{ValidProductId}*");
        }

        [Fact]
        public async Task GetProductAsync_WhenProductExists_ReturnsMappedDto()
        {
            // Arrange
            var product = SampleProduct();
            var dto = new ProductForGettingDto { Id = product.Id, ProductName = product.ProductName, Price = product.Price };

            _productRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
                .ReturnsAsync(product);

            _mapperMock
                .Setup(m => m.Map<ProductForGettingDto>(product))
                .Returns(dto);

            // Act
            var result = await _sut.GetProductAsync(ValidProductId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(ValidProductId);
            result.ProductName.Should().Be("Test Laptop");
        }

        // -----------------------------------------------------------------------
        // DeleteProductAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task DeleteProductAsync_WhenIdIsEmpty_ThrowsBadRequestException()
        {
            var act = () => _sut.DeleteProductAsync(Guid.Empty);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task DeleteProductAsync_WhenProductNotFound_ThrowsNotFoundException()
        {
            _productRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
                .ReturnsAsync((Product?)null);

            var act = () => _sut.DeleteProductAsync(ValidProductId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteProductAsync_WhenProductExists_RemovesAndSaves()
        {
            var product = SampleProduct();

            _productRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
                .ReturnsAsync(product);

            _productRepoMock
                .Setup(r => r.SaveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _sut.DeleteProductAsync(ValidProductId);

            // Assert
            result.Should().Be(1);
            _productRepoMock.Verify(r => r.Remove(product), Times.Once);
            _productRepoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // -----------------------------------------------------------------------
        // UpdateProductAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task UpdateProductAsync_WhenRequestIsNull_ThrowsBadRequestException()
        {
            var act = () => _sut.UpdateProductAsync(null!);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task UpdateProductAsync_WhenIdIsEmpty_ThrowsBadRequestException()
        {
            var request = new ProductForUpdatingDto { Id = Guid.Empty, ProductName = "X", Price = 1, Quantity = 1, CategoryId = ValidCategoryId, SupplierId = ValidSupplierId };

            var act = () => _sut.UpdateProductAsync(request);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*id is required*");
        }

        [Fact]
        public async Task UpdateProductAsync_WhenProductNameTooLong_ThrowsBadRequestException()
        {
            var request = new ProductForUpdatingDto
            {
                Id = ValidProductId,
                ProductName = new string('X', 101),
                Price = 1,
                Quantity = 1,
                CategoryId = ValidCategoryId,
                SupplierId = ValidSupplierId
            };

            var act = () => _sut.UpdateProductAsync(request);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*exceed 100*");
        }

        [Fact]
        public async Task UpdateProductAsync_WhenProductNotFound_ThrowsNotFoundException()
        {
            var request = new ProductForUpdatingDto
            {
                Id = ValidProductId,
                ProductName = "Valid Name",
                Price = 10,
                Quantity = 5,
                CategoryId = ValidCategoryId,
                SupplierId = ValidSupplierId
            };

            _categoryRepoMock
                .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>()))
                .ReturnsAsync(true);

            _supplierRepoMock
                .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>()))
                .ReturnsAsync(true);

            _productRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
                .ReturnsAsync((Product?)null);

            var act = () => _sut.UpdateProductAsync(request);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateProductAsync_WhenValid_UpdatesAndSaves()
        {
            var request = new ProductForUpdatingDto
            {
                Id = ValidProductId,
                ProductName = "Updated Laptop",
                Price = 1200m,
                Quantity = 5,
                CategoryId = ValidCategoryId,
                SupplierId = ValidSupplierId
            };

            var existingProduct = SampleProduct();

            _categoryRepoMock
                .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Category, bool>>>()))
                .ReturnsAsync(true);

            _supplierRepoMock
                .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>()))
                .ReturnsAsync(true);

            _productRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                    It.IsAny<bool>(),
                    It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Product>, IQueryable<Product>>>()))
                .ReturnsAsync(existingProduct);

            _productRepoMock
                .Setup(r => r.SaveAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _sut.UpdateProductAsync(request);

            // Assert
            result.Should().Be(1);
            _productRepoMock.Verify(r => r.Update(existingProduct), Times.Once);
            _productRepoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // -----------------------------------------------------------------------
        // GetAllProductsAsync — paged response
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetAllProductsAsync_WhenNoProducts_ReturnsEmptyPagedResponse()
        {
            _productRepoMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, object>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(), It.IsAny<bool>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, object>>[]>()))
                .ReturnsAsync((Enumerable.Empty<Product>(), 0));

            var result = await _sut.GetAllProductsAsync(DefaultPaging());

            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task GetAllProductsAsync_WhenProductsExist_ReturnsMappedPagedResponse()
        {
            var products = new List<Product> { SampleProduct() };
            var dtos = new List<ProductListForGettingDto> { new() { Id = ValidProductId, ProductName = "Test Laptop", Price = 999.99m } };

            _productRepoMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, object>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(), It.IsAny<bool>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, object>>[]>()))
                .ReturnsAsync((products.AsEnumerable(), 1));

            _mapperMock
                .Setup(m => m.Map<IEnumerable<ProductListForGettingDto>>(products))
                .Returns(dtos);

            var result = await _sut.GetAllProductsAsync(DefaultPaging());

            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(1);
            result.PageNumber.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        // -----------------------------------------------------------------------
        // GetAllProductsOfCategoryAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetAllProductsOfCategoryAsync_WhenCategoryIdIsEmpty_ThrowsBadRequestException()
        {
            var act = () => _sut.GetAllProductsOfCategoryAsync(Guid.Empty, DefaultPaging());

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*Category id*");
        }

        [Fact]
        public async Task GetAllProductsOfCategoryAsync_WhenCategoryHasProducts_ReturnsMappedResponse()
        {
            var categoryId = Guid.NewGuid();
            var products = new List<Product> { SampleProduct() };
            var dtos = new List<ProductListForGettingDto> { new() { Id = ValidProductId, ProductName = "Test Laptop" } };

            _productRepoMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, object>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(), It.IsAny<bool>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, object>>[]>()))
                .ReturnsAsync((products.AsEnumerable(), 1));

            _mapperMock
                .Setup(m => m.Map<IEnumerable<ProductListForGettingDto>>(products))
                .Returns(dtos);

            var result = await _sut.GetAllProductsOfCategoryAsync(categoryId, DefaultPaging());

            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(1);
        }

        // -----------------------------------------------------------------------
        // GetAllProductsOfSupplierAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetAllProductsOfSupplierAsync_WhenSupplierIdIsEmpty_ThrowsBadRequestException()
        {
            var act = () => _sut.GetAllProductsOfSupplierAsync(Guid.Empty, DefaultPaging());

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*Supplier id*");
        }

        [Fact]
        public async Task GetAllProductsOfSupplierAsync_WhenSupplierHasNoProducts_ReturnsEmptyResponse()
        {
            _productRepoMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, object>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(), It.IsAny<bool>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Product, object>>[]>()))
                .ReturnsAsync((Enumerable.Empty<Product>(), 0));

            var result = await _sut.GetAllProductsOfSupplierAsync(Guid.NewGuid(), DefaultPaging());

            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }
    }
}
