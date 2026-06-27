using Company524.API.Entities;
using Company524.API.Models.Authentication;
using Company524.API.Models.Category;
using Company524.IntegrationTests.Infrastructure;
using FluentAssertions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Company524.IntegrationTests.Tests;

/// <summary>
/// CATEGORIES ENDPOINT-ების INTEGRATION TESTS
///
/// ======================================================
/// ამ ტესტებში ვამოწმებთ:
/// ======================================================
/// 1. GET /api/categories        → ყველა კატეგორია (public)
/// 2. GET /api/categories/{id}   → კონკრეტული კატეგორია (public)
/// 3. POST /api/categories       → კატეგორიის შექმნა (Admin only)
/// 4. PUT /api/categories        → კატეგორიის განახლება (Admin only)
/// 5. DELETE /api/categories/{id} → კატეგორიის წაშლა (Admin only)
///
/// ======================================================
/// Authorization Flow:
/// ======================================================
/// Admin-ზე შეზღუდული endpoint-ების სატესტოდ:
/// 1. register admin user
/// 2. confirm email (ბაზაში პირდაპირ)
/// 3. login → access token
/// 4. Authorization: Bearer {token} header-ი ვამატებთ
/// 5. protected endpoint-ს ვეძახით
/// </summary>
public class CategoriesControllerTests : IntegrationTestBase
{
    private const string AdminEmail = "categoryadmin@company524.com";
    private const string AdminPassword = "Admin@1234";
    private const string CategoriesUrl = "/api/categories";
    private const string RegisterAdminUrl = "/api/auth/register-admin";
    private const string LoginUrl = "/api/auth/login";


    // -------------------------------------------------------
    // GET ALL CATEGORIES TESTS (Public — no auth needed)
    // -------------------------------------------------------

    [Fact]
    public async Task GetAllCategories_WhenEmpty_Returns200WithEmptyList()
    {
        // ARRANGE — ბაზა ცარიელია
        ClearDatabase();

        // ACT — Authorization header-ის გარეშე (public endpoint)
        var response = await Client.GetAsync(CategoriesUrl);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<PagedResponseWrapper<CategoryForGettingDto>>(content, JsonOptions);

        parsed!.IsSuccess.Should().BeTrue();
        parsed.Result.Items.Should().BeEmpty("ბაზა ცარიელია — შედეგები არ უნდა იყოს");
        parsed.Result.TotalCount.Should().Be(0);
    }

    [Fact]
    public async Task GetAllCategories_WithExistingData_Returns200WithCategories()
    {
        // ARRANGE — ბაზაში პირდაპირ ვამატებთ კატეგორიებს
        ClearDatabase();
        SeedCategories("Electronics", "Clothing", "Food");

        // ACT
        var response = await Client.GetAsync(CategoriesUrl);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<PagedResponseWrapper<CategoryForGettingDto>>(content, JsonOptions);

        // 3 კატეგორია სამივე უნდა დაბრუნდეს
        parsed!.Result.Items.Should().HaveCount(3);
        parsed.Result.TotalCount.Should().Be(3);
    }

    [Fact]
    public async Task GetAllCategories_WithPagination_ReturnsCorrectPage()
    {
        // ARRANGE
        ClearDatabase();
        SeedCategories("Cat1", "Cat2", "Cat3", "Cat4", "Cat5");

        // ACT — პირველი გვერდი, 2 ელემენტი
        var response = await Client.GetAsync($"{CategoriesUrl}?pageNumber=1&pageSize=2");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<PagedResponseWrapper<CategoryForGettingDto>>(content, JsonOptions);

        parsed!.Result.Items.Should().HaveCount(2, "pageSize=2 ამიტომ მხოლოდ 2 კატეგორია");
        parsed.Result.TotalCount.Should().Be(5, "სულ 5 კატეგორია გვაქვს ბაზაში");
    }


    // -------------------------------------------------------
    // GET CATEGORY BY ID TESTS (Public)
    // -------------------------------------------------------

    [Fact]
    public async Task GetCategoryById_WithExistingId_Returns200WithCategory()
    {
        // ARRANGE
        ClearDatabase();
        var categoryId = SeedSingleCategory("Electronics");

        // ACT
        var response = await Client.GetAsync($"{CategoriesUrl}/{categoryId}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<CommonResponseWrapper<CategoryForGettingDto>>(content, JsonOptions);

        parsed!.Result.CategoryName.Should().Be("Electronics");
        parsed.Result.Id.Should().Be(categoryId);
    }

    [Fact]
    public async Task GetCategoryById_WithNonExistentId_Returns404()
    {
        // ACT — ბაზაში არარსებული ID
        var nonExistentId = Guid.NewGuid();
        var response = await Client.GetAsync($"{CategoriesUrl}/{nonExistentId}");

        // ASSERT — ErrorHandlingMiddleware-მა 404 უნდა დააბრუნოს
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    // -------------------------------------------------------
    // CREATE CATEGORY TESTS (Admin only)
    // -------------------------------------------------------

    [Fact]
    public async Task CreateCategory_AsAdmin_Returns201AndCategoryExistsInDb()
    {
        // ARRANGE — admin-ად ავტორიზაცია
        ClearDatabase();
        await SetupAdminAuthAsync();

        var createRequest = new CategoryForCreatingDto
        {
            CategoryName = "New Electronics"
        };

        // ACT
        var response = await Client.PostAsJsonAsync(CategoriesUrl, createRequest);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.Created);  // 201

        // ბაზაში ჩაიწერა?
        using var db = GetDb();
        var categoryInDb = db.Categories
            .FirstOrDefault(c => c.CategoryName == "New Electronics");

        categoryInDb.Should().NotBeNull("კატეგორია ბაზაში უნდა შეიქმნას");
    }

    [Fact]
    public async Task CreateCategory_WithoutAuth_Returns401()
    {
        // ARRANGE — Authorization header-ი არ ვამატებთ
        Client.DefaultRequestHeaders.Authorization = null;

        // ACT
        var response = await Client.PostAsJsonAsync(CategoriesUrl, new CategoryForCreatingDto
        {
            CategoryName = "Unauthorized Category"
        });

        // ASSERT — [Authorize(Roles = "Admin")] ბლოკავს
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateCategory_WithNameExceeding50Chars_Returns400()
    {
        // ARRANGE
        ClearDatabase();
        await SetupAdminAuthAsync();

        // ACT — 51 სიმბოლო სახელი (ლიმიტი 50-ია)
        var response = await Client.PostAsJsonAsync(CategoriesUrl, new CategoryForCreatingDto
        {
            CategoryName = new string('A', 51)
        });

        // ASSERT — CategoryService-ი BadRequestException-ს სვამს
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    // -------------------------------------------------------
    // UPDATE CATEGORY TESTS (Admin only)
    // -------------------------------------------------------

    [Fact]
    public async Task UpdateCategory_AsAdmin_Returns200AndUpdatesInDb()
    {
        // ARRANGE
        ClearDatabase();
        await SetupAdminAuthAsync();
        var categoryId = SeedSingleCategory("Old Name");

        var updateRequest = new CategoryForUpdatingDto
        {
            Id = categoryId,
            CategoryName = "New Name"
        };

        // ACT
        var response = await Client.PutAsJsonAsync(CategoriesUrl, updateRequest);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // ბაზაში სახელი შეიცვალა?
        using var db = GetDb();
        var updatedCategory = db.Categories.Find(categoryId);
        updatedCategory!.CategoryName.Should().Be("New Name");
    }

    [Fact]
    public async Task UpdateCategory_WithNonExistentId_Returns404()
    {
        // ARRANGE
        ClearDatabase();
        await SetupAdminAuthAsync();

        // ACT — ბაზაში არარსებული ID
        var response = await Client.PutAsJsonAsync(CategoriesUrl, new CategoryForUpdatingDto
        {
            Id = Guid.NewGuid(),
            CategoryName = "Updated"
        });

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    // -------------------------------------------------------
    // DELETE CATEGORY TESTS (Admin only)
    // -------------------------------------------------------

    [Fact]
    public async Task DeleteCategory_AsAdmin_Returns200AndCategoryRemovedFromDb()
    {
        // ARRANGE
        ClearDatabase();
        await SetupAdminAuthAsync();
        var categoryId = SeedSingleCategory("To Be Deleted");

        // ACT
        var response = await Client.DeleteAsync($"{CategoriesUrl}/{categoryId}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // ბაზაში წაიშალა?
        using var db = GetDb();
        var deletedCategory = db.Categories.Find(categoryId);
        deletedCategory.Should().BeNull("კატეგორია წაშლილი უნდა იყოს");
    }

    [Fact]
    public async Task DeleteCategory_WithNonExistentId_Returns404()
    {
        // ARRANGE
        ClearDatabase();
        await SetupAdminAuthAsync();

        // ACT
        var response = await Client.DeleteAsync($"{CategoriesUrl}/{Guid.NewGuid()}");

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    // -------------------------------------------------------
    // Private Helpers
    // -------------------------------------------------------

    /// <summary>Admin user-ის registration + email confirm + login + auth header.</summary>
    private async Task SetupAdminAuthAsync()
    {
        // Register
        await Client.PostAsJsonAsync(RegisterAdminUrl, new RegistrationRequestDto
        {
            Email = AdminEmail,
            Password = AdminPassword
        });

        // Email confirm — ბაზაში პირდაპირ
        using (var db = GetDb())
        {
            var user = db.ApplicationUsers.FirstOrDefault(u => u.Email == AdminEmail);
            if (user != null)
            {
                user.EmailConfirmed = true;
                db.SaveChanges();
            }
        }

        // Login და token-ის მიღება
        var loginResponse = await Client.PostAsJsonAsync(LoginUrl, new LoginRequestDto
        {
            UserName = AdminEmail,
            Password = AdminPassword
        });

        var loginContent = await loginResponse.Content.ReadAsStringAsync();
        var loginResult = JsonSerializer.Deserialize<CommonResponseWrapper<LoginResponseDto>>(loginContent, JsonOptions);

        // Authorization header-ის დამატება
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", loginResult!.Result.AccessToken);
    }

    /// <summary>ბაზაში კატეგორიების დამატება სახელების მიხედვით.</summary>
    private void SeedCategories(params string[] names)
    {
        using var db = GetDb();
        foreach (var name in names)
        {
            db.Categories.Add(new Category { CategoryName = name });
        }
        db.SaveChanges();
    }

    /// <summary>ბაზაში ერთი კატეგორიის დამატება და მისი ID-ს დაბრუნება.</summary>
    private Guid SeedSingleCategory(string name)
    {
        using var db = GetDb();
        var category = new Category { CategoryName = name };
        db.Categories.Add(category);
        db.SaveChanges();
        return category.Id;
    }
}
