using Company524.API.Entities;
using Company524.API.Models.Authentication;
using Company524.API.Models.Product;
using Company524.IntegrationTests.Infrastructure;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Company524.IntegrationTests.Tests;

/// <summary>
/// PRODUCTS ENDPOINT-ების INTEGRATION TESTS
///
/// ======================================================
/// Products-ის სპეციფიკა:
/// ======================================================
/// - Product Category-ს და Supplier-ს ეკუთვნის (foreign keys)
/// - GET endpoints public-ია
/// - POST, PUT, DELETE — Admin-ზე შეზღუდული
/// - Pagination მხარდაჭერა
///
/// ======================================================
/// Integration Test-ების KEY ADVANTAGE:
/// ======================================================
/// FK constraints-ების ტესტირება!
/// Unit test-ებში InMemory Mock ეს ჩვეულებრივ "გვიარებს".
/// Integration test-ებში ბაზა REAL FK-ებს ამოწმებს.
/// (InMemory EF-ი FK validation-ს ასევე ახდენს SaveChanges-ზე)
/// </summary>
public class ProductsControllerTests : IntegrationTestBase
{
    private const string AdminEmail = "productadmin@company524.com";
    private const string AdminPassword = "Admin@1234";
    private const string ProductsUrl = "/api/products";
    private const string RegisterAdminUrl = "/api/auth/register-admin";
    private const string LoginUrl = "/api/auth/login";


    // -------------------------------------------------------
    // GET ALL PRODUCTS (Public)
    // -------------------------------------------------------

    [Fact]
    public async Task GetAllProducts_WhenEmpty_Returns200WithEmptyList()
    {
        // ARRANGE
        ClearDatabase();

        // ACT
        var response = await Client.GetAsync(ProductsUrl);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<PagedResponseWrapper<ProductListForGettingDto>>(content, JsonOptions);

        parsed!.Result.Items.Should().BeEmpty();
        parsed.Result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetAllProducts_WithProducts_Returns200WithProductList()
    {
        // ARRANGE
        ClearDatabase();
        var (categoryId, supplierId) = SeedCategoryAndSupplier();
        SeedProducts(categoryId, supplierId, count: 3);

        // ACT
        var response = await Client.GetAsync(ProductsUrl);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<PagedResponseWrapper<ProductListForGettingDto>>(content, JsonOptions);

        parsed!.Result.Items.Should().HaveCount(3);
        parsed.Result.TotalCount.Should().Be(3);
    }

    [Fact]
    public async Task GetAllProducts_WithPagination_ReturnsCorrectPage()
    {
        // ARRANGE — 10 product ვამატებთ, pageSize=3-ს ვიყენებთ
        ClearDatabase();
        var (categoryId, supplierId) = SeedCategoryAndSupplier();
        SeedProducts(categoryId, supplierId, count: 10);

        // ACT
        var response = await Client.GetAsync($"{ProductsUrl}?pageNumber=1&pageSize=3");

        // ASSERT
        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<PagedResponseWrapper<ProductListForGettingDto>>(content, JsonOptions);

        parsed!.Result.Items.Should().HaveCount(3, "pageSize=3 გამოვითხოვეთ");
        parsed.Result.TotalCount.Should().Be(10, "სულ 10 product გვაქვს");
        parsed.Result.PageNumber.Should().Be(1);
        parsed.Result.PageSize.Should().Be(3);
    }


    // -------------------------------------------------------
    // GET PRODUCT BY ID (Public)
    // -------------------------------------------------------

    [Fact]
    public async Task GetProduct_WithExistingId_Returns200WithProduct()
    {
        // ARRANGE
        ClearDatabase();
        var (categoryId, supplierId) = SeedCategoryAndSupplier();
        var productId = SeedSingleProduct("Test Product", 99.99m, 50, categoryId, supplierId);

        // ACT
        var response = await Client.GetAsync($"{ProductsUrl}/{productId}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<CommonResponseWrapper<ProductForGettingDto>>(content, JsonOptions);

        parsed!.Result.ProductName.Should().Be("Test Product");
        parsed.Result.Price.Should().Be(99.99m);
    }

    [Fact]
    public async Task GetProduct_WithNonExistentId_Returns404()
    {
        // ACT
        var response = await Client.GetAsync($"{ProductsUrl}/{Guid.NewGuid()}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    // -------------------------------------------------------
    // CREATE PRODUCT (Admin only)
    // -------------------------------------------------------

    [Fact]
    public async Task CreateProduct_AsAdmin_Returns201AndProductExistsInDb()
    {
        // ARRANGE
        ClearDatabase();
        await SetupAdminAuthAsync();
        var (categoryId, supplierId) = SeedCategoryAndSupplier();

        var createRequest = new ProductForCreatingDto
        {
            ProductName = "New Laptop",
            Price = 1299.99m,
            Quantity = 10,
            CategoryId = categoryId,
            SupplierId = supplierId
        };

        // ACT
        var response = await Client.PostAsJsonAsync(ProductsUrl, createRequest);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // ბაზაში ჩაიწერა?
        using var db = GetDb();
        var productInDb = db.Products
            .FirstOrDefault(p => p.ProductName == "New Laptop");

        productInDb.Should().NotBeNull("product ბაზაში უნდა შეიქმნას");
        productInDb!.Price.Should().Be(1299.99m);
        productInDb.Quantity.Should().Be(10);
    }

    [Fact]
    public async Task CreateProduct_WithoutAuth_Returns401()
    {
        // ARRANGE
        Client.DefaultRequestHeaders.Authorization = null;

        // ACT
        var response = await Client.PostAsJsonAsync(ProductsUrl, new ProductForCreatingDto
        {
            ProductName = "Unauthorized Product",
            Price = 100,
            Quantity = 5,
            CategoryId = Guid.NewGuid(),
            SupplierId = Guid.NewGuid()
        });

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


    // -------------------------------------------------------
    // DELETE PRODUCT (Admin only)
    // -------------------------------------------------------

    [Fact]
    public async Task DeleteProduct_AsAdmin_Returns204AndProductRemovedFromDb()
    {
        // ARRANGE
        ClearDatabase();
        await SetupAdminAuthAsync();
        var (categoryId, supplierId) = SeedCategoryAndSupplier();
        var productId = SeedSingleProduct("Delete Me", 50m, 1, categoryId, supplierId);

        // ACT
        var response = await Client.DeleteAsync($"{ProductsUrl}/{productId}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // ბაზაში წაიშალა?
        using var db = GetDb();
        var deletedProduct = db.Products.Find(productId);
        deletedProduct.Should().BeNull("product წაშლილი უნდა იყოს");
    }

    [Fact]
    public async Task DeleteProduct_WithNonExistentId_Returns404()
    {
        // ARRANGE
        ClearDatabase();
        await SetupAdminAuthAsync();

        // ACT
        var response = await Client.DeleteAsync($"{ProductsUrl}/{Guid.NewGuid()}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduct_WithoutAuth_Returns401()
    {
        // ARRANGE
        Client.DefaultRequestHeaders.Authorization = null;

        // ACT
        var response = await Client.DeleteAsync($"{ProductsUrl}/{Guid.NewGuid()}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


    // -------------------------------------------------------
    // UPDATE PRODUCT (Admin only)
    // -------------------------------------------------------

    [Fact]
    public async Task UpdateProduct_AsAdmin_Returns200AndUpdatesInDb()
    {
        // ARRANGE
        ClearDatabase();
        await SetupAdminAuthAsync();
        var (categoryId, supplierId) = SeedCategoryAndSupplier();
        var productId = SeedSingleProduct("Old Name", 100m, 10, categoryId, supplierId);

        var updateRequest = new ProductForUpdatingDto
        {
            Id = productId,
            ProductName = "Updated Name",
            Price = 150m,
            Quantity = 20,
            CategoryId = categoryId,
            SupplierId = supplierId
        };

        // ACT
        var response = await Client.PutAsJsonAsync(ProductsUrl, updateRequest);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var db = GetDb();
        var updated = db.Products.Find(productId);
        updated!.ProductName.Should().Be("Updated Name");
        updated.Price.Should().Be(150m);
        updated.Quantity.Should().Be(20);
    }


    // -------------------------------------------------------
    // GET PRODUCTS BY CATEGORY (Public)
    // -------------------------------------------------------

    [Fact]
    public async Task GetCategoryProducts_ReturnsOnlyProductsForThatCategory()
    {
        // ARRANGE
        ClearDatabase();
        var (categoryId1, supplierId) = SeedCategoryAndSupplier("Electronics", "SupplierA");
        var categoryId2 = SeedCategory("Clothing");

        // Electronics-ში 3 product, Clothing-ში 1 product
        SeedProducts(categoryId1, supplierId, count: 3);
        SeedSingleProduct("Shirt", 29.99m, 100, categoryId2, supplierId);

        // ACT — მხოლოდ Electronics-ის products
        var response = await Client.GetAsync($"/api/categories/{categoryId1}/products");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<PagedResponseWrapper<ProductListForGettingDto>>(content, JsonOptions);

        parsed!.Result.Items.Should().HaveCount(3, "Electronics-ს 3 product ჰქონდა");
        parsed.Result.TotalCount.Should().Be(3);
    }


    // -------------------------------------------------------
    // Private Helpers
    // -------------------------------------------------------

    private async Task SetupAdminAuthAsync()
    {
        await Client.PostAsJsonAsync(RegisterAdminUrl, new RegistrationRequestDto
        {
            Email = AdminEmail,
            Password = AdminPassword
        });

        using (var db = GetDb())
        {
            var user = db.ApplicationUsers.FirstOrDefault(u => u.Email == AdminEmail);
            if (user != null) { user.EmailConfirmed = true; db.SaveChanges(); }
        }

        var loginResponse = await Client.PostAsJsonAsync(LoginUrl, new LoginRequestDto
        {
            UserName = AdminEmail,
            Password = AdminPassword
        });

        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        var loginResult = JsonSerializer.Deserialize<CommonResponseWrapper<LoginResponseDto>>(loginContent, JsonOptions);

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginResult!.Result.AccessToken);
    }

    private (Guid categoryId, Guid supplierId) SeedCategoryAndSupplier(
        string categoryName = "Test Category",
        string supplierName = "Test Supplier")
    {
        using var db = GetDb();

        var category = new Category { CategoryName = categoryName };
        var supplier = new Supplier { SupplierName = supplierName };

        db.Categories.Add(category);
        db.Suppliers.Add(supplier);
        db.SaveChanges();

        return (category.Id, supplier.Id);
    }

    private Guid SeedCategory(string name)
    {
        using var db = GetDb();
        var category = new Category { CategoryName = name };
        db.Categories.Add(category);
        db.SaveChanges();
        return category.Id;
    }

    private void SeedProducts(Guid categoryId, Guid supplierId, int count)
    {
        using var db = GetDb();
        for (int i = 1; i <= count; i++)
        {
            db.Products.Add(new Product
            {
                ProductName = $"Product {i}",
                Price = 10m * i,
                Quantity = i * 5,
                CategoryId = categoryId,
                SupplierId = supplierId
            });
        }
        db.SaveChanges();
    }

    private Guid SeedSingleProduct(string name, decimal price, int qty, Guid categoryId, Guid supplierId)
    {
        using var db = GetDb();
        var product = new Product
        {
            ProductName = name,
            Price = price,
            Quantity = qty,
            CategoryId = categoryId,
            SupplierId = supplierId
        };
        db.Products.Add(product);
        db.SaveChanges();
        return product.Id;
    }
}
