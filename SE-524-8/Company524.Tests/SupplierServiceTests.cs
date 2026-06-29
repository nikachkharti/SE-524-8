using Company524.Domain.Entities;
using Company524.Application.Exceptions;
using Company524.Application.Models.Common;
using Company524.Application.Models.Supplier;
using Company524.Application.Contracts.Persistence;
using Company524.Application.Service;
using FluentAssertions;
using MapsterMapper;
using Moq;

namespace Company524.Tests
{
    public class SupplierServiceTests
    {
        private readonly Mock<ISupplierRepository> _supplierRepoMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly SupplierService _sut;
        private static readonly Guid ValidId = Guid.NewGuid();
        private static Supplier SampleSupplier() => new() { Id = ValidId, SupplierName = "TechCorp" };

        public SupplierServiceTests()
        {
            _sut = new SupplierService(_supplierRepoMock.Object, _mapperMock.Object);
        }


        // -----------------------------------------------------------------------
        // CreateSupplierAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task CreateSupplierAsync_WhenModelIsNull_ThrowsBadRequestException()
        {
            var act = () => _sut.CreateSupplierAsync(null!);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*required*");
        }

        [Fact]
        public async Task CreateSupplierAsync_WhenNameExceeds100Chars_ThrowsBadRequestException()
        {
            var model = new SupplierForCreatingDto { SupplierName = new string('Z', 101) };

            var act = () => _sut.CreateSupplierAsync(model);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*exceed 100*");
        }

        [Fact]
        public async Task CreateSupplierAsync_WhenNameIsExactly100Chars_DoesNotThrow()
        {
            // Boundary value: 100 chars must be accepted
            var model = new SupplierForCreatingDto { SupplierName = new string('Z', 100) };
            var supplier = SampleSupplier();

            _mapperMock.Setup(m => m.Map<Supplier>(model)).Returns(supplier);
            _supplierRepoMock.Setup(r => r.AddAsync(supplier)).Returns(Task.CompletedTask);
            _supplierRepoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<SupplierForGettingDto>(supplier)).Returns(new SupplierForGettingDto());

            var act = () => _sut.CreateSupplierAsync(model);

            await act.Should().NotThrowAsync();
        }

        [Fact]
        public async Task CreateSupplierAsync_WhenValid_SavesAndReturnsMappedDto()
        {
            var model = new SupplierForCreatingDto { SupplierName = "TechCorp" };
            var supplier = SampleSupplier();
            var dto = new SupplierForGettingDto { Id = ValidId, SupplierName = "TechCorp" };

            _mapperMock.Setup(m => m.Map<Supplier>(model)).Returns(supplier);
            _supplierRepoMock.Setup(r => r.AddAsync(supplier)).Returns(Task.CompletedTask);
            _supplierRepoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<SupplierForGettingDto>(supplier)).Returns(dto);

            var result = await _sut.CreateSupplierAsync(model);

            result.SupplierName.Should().Be("TechCorp");
            _supplierRepoMock.Verify(r => r.AddAsync(supplier), Times.Once);
            _supplierRepoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // -----------------------------------------------------------------------
        // GetSupplierByIdAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetSupplierByIdAsync_WhenIdIsEmpty_ThrowsBadRequestException()
        {
            var act = () => _sut.GetSupplierByIdAsync(Guid.Empty);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*id is required*");
        }

        [Fact]
        public async Task GetSupplierByIdAsync_WhenSupplierNotFound_ThrowsNotFoundException()
        {
            _supplierRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Supplier>, IQueryable<Supplier>>>()))
                .ReturnsAsync((Supplier?)null);

            var act = () => _sut.GetSupplierByIdAsync(ValidId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("*not found*");
        }

        [Fact]
        public async Task GetSupplierByIdAsync_WhenSupplierExists_ReturnsMappedDto()
        {
            var supplier = SampleSupplier();
            var dto = new SupplierForGettingDto { Id = ValidId, SupplierName = "TechCorp" };

            _supplierRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Supplier>, IQueryable<Supplier>>>()))
                .ReturnsAsync(supplier);

            _mapperMock.Setup(m => m.Map<SupplierForGettingDto>(supplier)).Returns(dto);

            var result = await _sut.GetSupplierByIdAsync(ValidId);

            result.SupplierName.Should().Be("TechCorp");
        }

        // -----------------------------------------------------------------------
        // GetAllSuppliersAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task GetAllSuppliersAsync_WhenNoSuppliers_ReturnsEmptyResponse()
        {
            _supplierRepoMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, object>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(), It.IsAny<bool>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, object>>[]>()))
                .ReturnsAsync((Enumerable.Empty<Supplier>(), 0));

            var result = await _sut.GetAllSuppliersAsync(new PagedRequestDto());

            result.Items.Should().BeEmpty();
            result.TotalCount.Should().Be(0);
        }

        [Fact]
        public async Task GetAllSuppliersAsync_WhenSuppliersExist_ReturnsMappedPagedResponse()
        {
            var suppliers = new List<Supplier> { SampleSupplier() };
            var dtos = new List<SupplierForGettingDto> { new() { Id = ValidId, SupplierName = "TechCorp" } };

            _supplierRepoMock
                .Setup(r => r.GetAllAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, object>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(), It.IsAny<bool>(),
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, object>>[]>()))
                .ReturnsAsync((suppliers.AsEnumerable(), 1));

            _mapperMock
                .Setup(m => m.Map<IEnumerable<SupplierForGettingDto>>(suppliers))
                .Returns(dtos);

            var result = await _sut.GetAllSuppliersAsync(new PagedRequestDto { PageNumber = 1, PageSize = 10 });

            result.Items.Should().HaveCount(1);
            result.TotalCount.Should().Be(1);
        }

        // -----------------------------------------------------------------------
        // DeleteSupplierAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task DeleteSupplierAsync_WhenIdIsEmpty_ThrowsBadRequestException()
        {
            var act = () => _sut.DeleteSupplierAsync(Guid.Empty);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task DeleteSupplierAsync_WhenSupplierNotFound_ThrowsNotFoundException()
        {
            _supplierRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Supplier>, IQueryable<Supplier>>>()))
                .ReturnsAsync((Supplier?)null);

            var act = () => _sut.DeleteSupplierAsync(ValidId);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task DeleteSupplierAsync_WhenSupplierExists_RemovesAndSaves()
        {
            var supplier = SampleSupplier();

            _supplierRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Supplier>, IQueryable<Supplier>>>()))
                .ReturnsAsync(supplier);

            _supplierRepoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            await _sut.DeleteSupplierAsync(ValidId);

            _supplierRepoMock.Verify(r => r.Remove(supplier), Times.Once);
            _supplierRepoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        // -----------------------------------------------------------------------
        // UpdateSupplierAsync
        // -----------------------------------------------------------------------

        [Fact]
        public async Task UpdateSupplierAsync_WhenModelIsNull_ThrowsBadRequestException()
        {
            var act = () => _sut.UpdateSupplierAsync(null!);

            await act.Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task UpdateSupplierAsync_WhenIdIsEmpty_ThrowsBadRequestException()
        {
            var model = new SupplierForUpdatingDto { Id = Guid.Empty, SupplierName = "Valid" };

            var act = () => _sut.UpdateSupplierAsync(model);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*id is required*");
        }

        [Fact]
        public async Task UpdateSupplierAsync_WhenNameExceeds100Chars_ThrowsBadRequestException()
        {
            var model = new SupplierForUpdatingDto { Id = ValidId, SupplierName = new string('X', 101) };

            var act = () => _sut.UpdateSupplierAsync(model);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("*exceed 100*");
        }

        [Fact]
        public async Task UpdateSupplierAsync_WhenSupplierNotFound_ThrowsNotFoundException()
        {
            var model = new SupplierForUpdatingDto { Id = ValidId, SupplierName = "Valid" };

            _supplierRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Supplier>, IQueryable<Supplier>>>()))
                .ReturnsAsync((Supplier?)null);

            var act = () => _sut.UpdateSupplierAsync(model);

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task UpdateSupplierAsync_WhenValid_UpdatesAndReturnsMappedDto()
        {
            var model = new SupplierForUpdatingDto { Id = ValidId, SupplierName = "Updated Corp" };
            var existing = SampleSupplier();
            var dto = new SupplierForGettingDto { Id = ValidId, SupplierName = "Updated Corp" };

            _supplierRepoMock
                .Setup(r => r.GetAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<Supplier, bool>>>(),
                    It.IsAny<bool>(), It.IsAny<CancellationToken>(),
                    It.IsAny<Func<IQueryable<Supplier>, IQueryable<Supplier>>>()))
                .ReturnsAsync(existing);

            _supplierRepoMock.Setup(r => r.SaveAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _mapperMock.Setup(m => m.Map<SupplierForGettingDto>(existing)).Returns(dto);

            var result = await _sut.UpdateSupplierAsync(model);

            result.SupplierName.Should().Be("Updated Corp");
            _supplierRepoMock.Verify(r => r.Update(existing), Times.Once);
            _supplierRepoMock.Verify(r => r.SaveAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }

}
