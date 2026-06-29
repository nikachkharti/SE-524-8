using Company524.Infrastructure.Data;
using Company524.Application.Models.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Company524.Tests.Integration
{

    /// <summary>
    /// ეს არის საბაზო (base) კლასი ყველა ჩვენი integration test-ისთვის.
    ///
    /// WHAT IS A BASE CLASS?
    /// როდესაც ყველა test-ს ერთი და იგივე კოდი სჭირდება
    /// (მაგ. HttpClient-ის შექმნა, login-ი, database-ის გასუფთავება),
    /// ჩვენ ამ კოდს ერთ ადგილას ვწერთ — base class-ში.
    /// ყველა test class შემდეგ ამ class-ს "inherits" (: IntegrationTestBase).
    ///
    /// WHAT DOES IT PROVIDE?
    /// - Client     → HTTP მოთხოვნების გასაგზავნი
    /// - Factory    → ტესტ სერვერი
    /// - LoginAsync → ავტორიზაცია და token-ის მიღება
    /// - GetDb      → ბაზაზე პირდაპირი წვდომა (verification-ისთვის)
    /// - ClearDatabase → ტესტებს შორის ბაზის გასუფთავება
    /// </summary>
    public class IntegrationTestBase : IDisposable
    {
        // CustomWebApplicationFactory ქმნის ჩვენს ტესტ სერვერს
        protected readonly CustomWebApplicationFactory Factory;

        // HttpClient — ამით ვაგზავნით HTTP request-ებს ჩვენს API-ზე
        protected readonly HttpClient Client;

        // JSON სერიალიზაციის პარამეტრები
        protected static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true  // "accessToken" = "AccessToken"
        };

        public IntegrationTestBase()
        {
            Factory = new CustomWebApplicationFactory();

            // CreateClient() — ქმნის HttpClient-ს, რომელიც ჩვენს
            // in-memory სერვერზე გზავნის request-ებს (http://localhost)
            Client = Factory.CreateClient();
        }


        /// <summary>
        /// API-ში ავტორიზაცია და JWT access token-ის მიღება.
        ///
        /// HOW IT WORKS:
        /// 1. POST /api/auth/login გავუგზავნოთ
        /// 2. Response-დან ამოვიღოთ accessToken
        /// 3. შემდეგი request-ებისთვის Authorization header-ში ჩავდოთ
        ///
        /// WHY?
        /// ჩვენი endpoint-ების ნაწილი [Authorize] attribute-ს საჭიროებს.
        /// ამ გარეშე 401 Unauthorized-ს დავიბრუნებთ.
        /// </summary>
        protected async Task<string> LoginAndGetTokenAsync(string email, string password)
        {
            var loginRequest = new LoginRequestDto
            {
                UserName = email,
                Password = password
            };

            var response = await Client.PostAsJsonAsync("/api/auth/login", loginRequest);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var parsed = JsonSerializer.Deserialize<CommonResponseWrapper<LoginResponseDto>>(content, JsonOptions);

            return parsed!.Result.AccessToken;
        }

        /// <summary>
        /// HttpClient-ის Authorization header-ში JWT token-ის ჩამატება.
        ///
        /// გამოყენება: await SetAuthHeaderAsync("admin@test.com", "Password1!");
        /// ამის შემდეგ Client-ის ყველა request-ი [Authorize] endpoint-ებზე
        /// წარმატებით მუშაობს.
        /// </summary>
        protected async Task SetAuthHeaderAsync(string email, string password)
        {
            var token = await LoginAndGetTokenAsync(email, password);
            Client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// ბაზაზე პირდაპირი წვდომა — verification-ისთვის.
        ///
        /// WHY DO WE NEED THIS?
        /// მაგალითად: POST /api/categories გამოვიძახეთ.
        /// Controller-მა 201 Created დაგვიბრუნა.
        /// მაგრამ ნამდვილად ჩაიწერა ბაზაში? ამის შესამოწმებლად
        /// GetDb()-ით ვუკავშირდებით ბაზას პირდაპირ.
        /// </summary>
        protected ApplicationDbContext GetDb()
        {
            var scope = Factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }


        /// <summary>
        /// ბაზის გასუფთავება ყოველი ტესტის შემდეგ.
        ///
        /// WHY IS THIS IMPORTANT?
        /// test A-მ ბაზაში Category შექმნა.
        /// test B ხსნის ყველა Category-ს სიას.
        /// თუ ბაზა არ გასუფთავდა — test B-ს test A-ს მიერ
        /// შექმნილი category-ც ექნება, რაც ჩვენ არ გვინდა.
        ///
        /// გამოსავალი: ყოველი ტესტის წინ / შემდეგ ვასუფთავებთ ბაზას.
        /// </summary>
        protected void ClearDatabase()
        {
            using var db = GetDb();
            db.RefreshTokens.RemoveRange(db.RefreshTokens);
            db.Products.RemoveRange(db.Products);
            db.Categories.RemoveRange(db.Categories);
            db.Suppliers.RemoveRange(db.Suppliers);
            db.ApplicationUsers.RemoveRange(db.ApplicationUsers);
            db.SaveChanges();
        }


        // IDisposable.Dispose() — xUnit ამ method-ს ყოველი test-ის შემდეგ გამოიძახებს
        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }

    public class CommonResponseWrapper<T>
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int HttpStatusCode { get; set; }
        public T Result { get; set; }
    }

    public class PagedResponseWrapper<T>
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public int HttpStatusCode { get; set; }
        public PagedResult<T> Result { get; set; }
    }

    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

}
