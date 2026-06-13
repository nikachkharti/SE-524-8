using Company524.API.Models.Authentication;
using Company524.API.Models.Category;
using Company524.API.Models.Product;
using Company524.API.Models.Supplier;
using Swashbuckle.AspNetCore.Filters;

namespace Company524.API;

public sealed record ProductForCreatingDtoExample : IExamplesProvider<ProductForCreatingDto>
{
    public ProductForCreatingDto GetExamples()
    {
        return new ProductForCreatingDto()
        {
            ProductName = "Apple iPhone 14 Pro Max",
            Price = 1099.99m,
            Quantity = 10,
            CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000001"),
            SupplierId = Guid.Parse("22222222-0000-0000-0000-000000000001")
        };
    }
}

public sealed record ProductForUpdatingDtoExample : IExamplesProvider<ProductForUpdatingDto>
{
    public ProductForUpdatingDto GetExamples()
    {
        return new ProductForUpdatingDto()
        {
            ProductName = "Updated Apple iPhone 14 Pro Max",
            Price = 1099.99m,
            Quantity = 10,
            CategoryId = Guid.Parse("11111111-0000-0000-0000-000000000001"),
            SupplierId = Guid.Parse("22222222-0000-0000-0000-000000000001")
        };
    }
}


public sealed record SupplierForCreatingDtoExample : IExamplesProvider<SupplierForCreatingDto>
{
    public SupplierForCreatingDto GetExamples() =>
        new SupplierForCreatingDto() { SupplierName = "Test Supplier" };
}


public sealed record SupplierForUpdatingDtoExample : IExamplesProvider<SupplierForUpdatingDto>
{
    public SupplierForUpdatingDto GetExamples() =>
        new SupplierForUpdatingDto() { Id = Guid.Parse("22222222-0000-0000-0000-000000000001"), SupplierName = "Updated Test Supplier" };
}

public sealed record CategoryForCreatingDtoExample : IExamplesProvider<CategoryForCreatingDto>
{
    public CategoryForCreatingDto GetExamples() =>
        new CategoryForCreatingDto() { CategoryName = "Test Category" };
}

public sealed record CategoryForUpdatingDtoExample : IExamplesProvider<CategoryForUpdatingDto>
{
    public CategoryForUpdatingDto GetExamples() =>
        new CategoryForUpdatingDto() { Id = Guid.Parse("11111111-0000-0000-0000-000000000001"), CategoryName = "Updated Test Category" };
}

public sealed record RegistrationRequestDtoExample : IExamplesProvider<RegistrationRequestDto>
{
    public RegistrationRequestDto GetExamples() =>
        new RegistrationRequestDto() { Email = "admin@gmail.com", Password = "Admin123456789!" };
}

public sealed record LoginRequestDtoExample : IExamplesProvider<LoginRequestDto>
{
    public LoginRequestDto GetExamples() =>
        new LoginRequestDto() { UserName = "admin@gmail.com", Password = "Admin123456789!" };
}