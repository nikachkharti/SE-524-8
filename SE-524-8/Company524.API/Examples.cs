using Company524.API.Models.Product;
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



