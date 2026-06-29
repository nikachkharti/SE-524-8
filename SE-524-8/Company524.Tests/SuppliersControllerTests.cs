using Company524.API.Controllers;
using Company524.Application.Models.Common;
using Company524.Application.Models.Supplier;
using Company524.Application.Contracts.Service;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Company524.Tests
{
    public class SuppliersControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock = new();
        private readonly Mock<ISupplierService> _supplierServiceMock = new();
        private readonly SuppliersController _sut;

        private static readonly Guid ValidId = Guid.NewGuid();
        private static readonly PagedRequestDto DefaultPaging = new() { PageNumber = 1, PageSize = 10 };

        public SuppliersControllerTests()
        {
            _sut = new SuppliersController(_productServiceMock.Object, _supplierServiceMock.Object);
        }

        [Fact]
        public async Task GetSuppliers_WhenCalled_Returns200()
        {
            var pagedResponse = new PagedResponseDto<SupplierForGettingDto>
            {
                Items = [new SupplierForGettingDto { Id = ValidId, SupplierName = "TechCorp" }],
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };

            _supplierServiceMock.Setup(s => s.GetAllSuppliersAsync(DefaultPaging)).ReturnsAsync(pagedResponse);

            var actionResult = await _sut.GetSuppliers(DefaultPaging);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task GetSupplier_WhenExists_Returns200()
        {
            var dto = new SupplierForGettingDto { Id = ValidId, SupplierName = "TechCorp" };
            _supplierServiceMock.Setup(s => s.GetSupplierByIdAsync(ValidId)).ReturnsAsync(dto);

            var actionResult = await _sut.GetSupplier(ValidId);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task CreateSupplier_WhenValid_Returns201()
        {
            var model = new SupplierForCreatingDto { SupplierName = "New Supplier" };
            var dto = new SupplierForGettingDto { Id = ValidId, SupplierName = "New Supplier" };
            _supplierServiceMock.Setup(s => s.CreateSupplierAsync(model)).ReturnsAsync(dto);

            var actionResult = await _sut.CreateSupplier(model);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task DeleteSupplier_WhenExists_Returns200()
        {
            _supplierServiceMock.Setup(s => s.DeleteSupplierAsync(ValidId)).Returns(Task.CompletedTask);

            var actionResult = await _sut.DeleteSupplier(ValidId);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task UpdateSupplier_WhenValid_Returns200()
        {
            var model = new SupplierForUpdatingDto { Id = ValidId, SupplierName = "Updated" };
            var dto = new SupplierForGettingDto { Id = ValidId, SupplierName = "Updated" };
            _supplierServiceMock.Setup(s => s.UpdateSupplierAsync(model)).ReturnsAsync(dto);

            var actionResult = await _sut.UpdateSupplier(model);

            var result = actionResult.Should().BeOfType<ObjectResult>().Subject;
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
        }
    }
}
