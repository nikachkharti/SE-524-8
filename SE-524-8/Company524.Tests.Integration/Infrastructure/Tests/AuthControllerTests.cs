using Company524.API.Models.Authentication;
using Company524.IntegrationTests.Infrastructure;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Company524.IntegrationTests.Tests;

/// <summary>
/// AUTH ENDPOINT-ების INTEGRATION TESTS
///
/// ======================================================
/// რა განსხვავება არის UNIT TEST-სა და INTEGRATION TEST-ს შორის?
/// ======================================================
///
/// UNIT TEST (ის რაც უკვე გაქვს):
///   - ტესტავს ერთ კლასს (მაგ. AuthService) სხვებისგან იზოლირებულად
///   - სხვა dependencies-ები (UserManager, DbContext...) Mock-ებია
///   - სწრაფია, მარტივი
///   - კითხვა: "ეს ლოგიკა სწორია?"
///
/// INTEGRATION TEST (ეს ახლა):
///   - ტესტავს ᲛᲗᲔᲚ CHAIN-ს: HTTP Request → Controller → Service → Database
///   - Mock-ები მინიმალურია (მხოლოდ გარე სერვისები: Email, SMTP)
///   - ნელია, მაგრამ ბევრად უფრო realistically ამოწმებს
///   - კითხვა: "ყველა ნაწილი ერთად სწორად მუშაობს?"
///
/// ======================================================
/// ანალოგია (საწყობი):
/// ======================================================
///
/// 🔧 UNIT TEST = ცალ-ცალკე ამოწმებ: კარი იხსნება? სარკმელი იხსნება?
/// 🏠 INTEGRATION TEST = ამოწმებ: სახლი სწორად არის აშენებული?
///                        კარი გაღებისას ოთახი ნამდვილად ხდება ხელმისაწვდომი?
///
/// </summary>
public class AuthControllerTests : IntegrationTestBase
{
    // -------------------------------------------------------
    // Test constants — ტესტებში გამოსაყენებელი მონაცემები
    // -------------------------------------------------------
    private const string ValidEmail = "testadmin@company524.com";
    private const string ValidPassword = "Admin@1234";
    private const string RegisterAdminUrl = "/api/auth/register-admin";
    private const string LoginUrl = "/api/auth/login";
    private const string RefreshTokenUrl = "/api/auth/refresh-token";
    private const string RevokeTokenUrl = "/api/auth/revoke-token";


    // -------------------------------------------------------
    // REGISTER TESTS
    // -------------------------------------------------------

    [Fact]
    public async Task RegisterAdmin_WithValidData_Returns201AndUserId()
    {
        // -------------------------------------------------------
        // ARRANGE — მოვემზადოთ
        // -------------------------------------------------------
        // request-ი, რომელსაც API-ს გავუგზავნით
        ClearDatabase();

        var request = new RegistrationRequestDto
        {
            Email = ValidEmail,
            Password = ValidPassword
        };

        // -------------------------------------------------------
        // ACT — ვმოქმედოთ
        // -------------------------------------------------------
        // PostAsJsonAsync = HTTP POST request-ის გაგზავნა JSON body-ით
        var response = await Client.PostAsJsonAsync(RegisterAdminUrl, request);

        // -------------------------------------------------------
        // ASSERT — შევამოწმოთ
        // -------------------------------------------------------

        // 1. HTTP Status Code სწორია?
        response.StatusCode.Should().Be(HttpStatusCode.Created);  // 201

        // 2. Response body-ს ვკითხულობთ
        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<CommonResponseWrapper<string>>(content, JsonOptions);

        // 3. Response-ი წარმატებულია?
        parsed!.IsSuccess.Should().BeTrue();

        // 4. UserId დაბრუნდა?
        parsed.Result.Should().NotBeNullOrEmpty("registration-მა user-ის ID უნდა დააბრუნოს");

        // 5. ბაზაში ნამდვილად ჩაიწერა?
        //    ← ეს UNIT TEST-ში შეუძლებელია! integration-ის ძლიერება სწორედ ეს არის.
        using var db = GetDb();
        var userInDb = db.ApplicationUsers.FirstOrDefault(u => u.Email == ValidEmail);
        userInDb.Should().NotBeNull("user ბაზაში უნდა არსებობდეს");
        userInDb!.EmailConfirmed.Should().BeFalse("email-ი ჯერ არ არის კონფირმირებული");
    }

    [Fact]
    public async Task RegisterAdmin_WithDuplicateEmail_Returns400()
    {
        // ARRANGE
        ClearDatabase();

        var request = new RegistrationRequestDto
        {
            Email = ValidEmail,
            Password = ValidPassword
        };

        // პირველი registration — წარმატებული
        await Client.PostAsJsonAsync(RegisterAdminUrl, request);

        // ACT — იგივე email-ით მეორედ ვცდით
        var response = await Client.PostAsJsonAsync(RegisterAdminUrl, request);

        // ASSERT — API-მ 400 Bad Request უნდა დააბრუნოს
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RegisterAdmin_WithWeakPassword_Returns400()
    {
        // ARRANGE
        ClearDatabase();

        var request = new RegistrationRequestDto
        {
            Email = ValidEmail,
            Password = "123"  // ← ძალიან სუსტი პაროლი
        };

        // ACT
        var response = await Client.PostAsJsonAsync(RegisterAdminUrl, request);

        // ASSERT — Identity Validator-ი უარს იტყვის
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    // -------------------------------------------------------
    // LOGIN TESTS
    // -------------------------------------------------------

    [Fact]
    public async Task Login_WithConfirmedEmail_Returns200AndTokens()
    {
        // ARRANGE
        ClearDatabase();
        await RegisterAndConfirmEmailAsync(ValidEmail, ValidPassword);

        var loginRequest = new LoginRequestDto
        {
            UserName = ValidEmail,
            Password = ValidPassword
        };

        // ACT
        var response = await Client.PostAsJsonAsync(LoginUrl, loginRequest);

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<CommonResponseWrapper<LoginResponseDto>>(content, JsonOptions);

        // API-მ access token და refresh token უნდა დააბრუნოს
        parsed!.Result.AccessToken.Should().NotBeNullOrEmpty("JWT token-ი გენერირებული უნდა იყოს");
        parsed.Result.RefreshToken.Should().NotBeNullOrEmpty("refresh token-ი გენერირებული უნდა იყოს");
    }

    [Fact]
    public async Task Login_WithUnconfirmedEmail_Returns401()
    {
        // ARRANGE — register გავაკეთეთ, მაგრამ email-ი ΝΟΤ confirmed
        ClearDatabase();
        await Client.PostAsJsonAsync(RegisterAdminUrl, new RegistrationRequestDto
        {
            Email = ValidEmail,
            Password = ValidPassword
        });

        // ACT — login-ს ვცდით ისე, რომ email არ გვაქვს confirmed
        var response = await Client.PostAsJsonAsync(LoginUrl, new LoginRequestDto
        {
            UserName = ValidEmail,
            Password = ValidPassword
        });

        // ASSERT — AuthService-მა UnauthorizedException უნდა სვიოს
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithWrongPassword_Returns400()
    {
        // ARRANGE
        ClearDatabase();
        await RegisterAndConfirmEmailAsync(ValidEmail, ValidPassword);

        // ACT — სხვა პაროლი
        var response = await Client.PostAsJsonAsync(LoginUrl, new LoginRequestDto
        {
            UserName = ValidEmail,
            Password = "WrongPassword@999"
        });

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Login_WithNonExistentUser_Returns400()
    {
        // ARRANGE
        ClearDatabase();

        // ACT — არ არსებული მომხმარებელი
        var response = await Client.PostAsJsonAsync(LoginUrl, new LoginRequestDto
        {
            UserName = "nobody@company524.com",
            Password = ValidPassword
        });

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }


    // -------------------------------------------------------
    // REFRESH TOKEN TESTS
    // -------------------------------------------------------

    [Fact]
    public async Task RefreshToken_WithValidToken_Returns200AndNewTokens()
    {
        // ARRANGE — login გავაკეთოთ და refresh token ავიღოთ
        ClearDatabase();
        await RegisterAndConfirmEmailAsync(ValidEmail, ValidPassword);
        var loginResult = await LoginUserAsync(ValidEmail, ValidPassword);

        // ACT — refresh token გამოვიყენოთ ახალი token-ების მისაღებად
        var response = await Client.PostAsJsonAsync(RefreshTokenUrl, new RefreshTokenRequestDto
        {
            RefreshToken = loginResult.RefreshToken
        });

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<CommonResponseWrapper<LoginResponseDto>>(content, JsonOptions);

        // ახალი token-ები დაბრუნდა?
        parsed!.Result.AccessToken.Should().NotBeNullOrEmpty();
        parsed.Result.RefreshToken.Should().NotBeNullOrEmpty();

        // ახალი refresh token ძველისგან განსხვავებული უნდა იყოს (rotation)
        parsed.Result.RefreshToken.Should()
            .NotBe(loginResult.RefreshToken, "refresh token-ი ყოველ გამოყენებაზე იცვლება (rotation)");
    }

    [Fact]
    public async Task RefreshToken_WithInvalidToken_Returns400()
    {
        // ACT — არ არსებული refresh token
        var response = await Client.PostAsJsonAsync(RefreshTokenUrl, new RefreshTokenRequestDto
        {
            RefreshToken = "this-is-not-a-real-token"
        });

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task RefreshToken_WithAlreadyUsedToken_Returns401()
    {
        // ARRANGE
        ClearDatabase();
        await RegisterAndConfirmEmailAsync(ValidEmail, ValidPassword);
        var loginResult = await LoginUserAsync(ValidEmail, ValidPassword);

        // პირველი გამოყენება — OK
        await Client.PostAsJsonAsync(RefreshTokenUrl, new RefreshTokenRequestDto
        {
            RefreshToken = loginResult.RefreshToken
        });

        // ACT — იგივე token-ს ხელმეორე გამოყენება
        var response = await Client.PostAsJsonAsync(RefreshTokenUrl, new RefreshTokenRequestDto
        {
            RefreshToken = loginResult.RefreshToken
        });

        // ASSERT — revoked token-ი 401 უნდა დააბრუნოს
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


    // -------------------------------------------------------
    // REVOKE TOKEN TESTS
    // -------------------------------------------------------

    [Fact]
    public async Task RevokeToken_WithValidAuthenticatedUser_Returns200()
    {
        // ARRANGE
        ClearDatabase();
        await RegisterAndConfirmEmailAsync(ValidEmail, ValidPassword);
        var loginResult = await LoginUserAsync(ValidEmail, ValidPassword);

        // Authorization header-ი ვამატებთ
        Client.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginResult.AccessToken);

        // ACT
        var response = await Client.PostAsJsonAsync(RevokeTokenUrl, new RefreshTokenRequestDto
        {
            RefreshToken = loginResult.RefreshToken
        });

        // ASSERT
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        // ბაზაში refresh token-ი გაუქმდა?
        using var db = GetDb();
        var revokedToken = db.RefreshTokens
            .FirstOrDefault(t => t.Token == loginResult.RefreshToken);

        revokedToken!.RevokedAt.Should().NotBeNull("RevokedAt მნიშვნელობა უნდა დაყენდეს");
    }

    [Fact]
    public async Task RevokeToken_WithoutAuthentication_Returns401()
    {
        // ACT — Authorization header-ის გარეშე
        Client.DefaultRequestHeaders.Authorization = null;

        var response = await Client.PostAsJsonAsync(RevokeTokenUrl, new RefreshTokenRequestDto
        {
            RefreshToken = "any-token"
        });

        // ASSERT — [Authorize] attribute-ი 401-ს დააბრუნებს
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }


    // -------------------------------------------------------
    // Private helper methods
    // -------------------------------------------------------

    /// <summary>
    /// Registration + Email Confirmation (ბაზაში პირდაპირ).
    /// Integration test-ებში Email-ის კონფირმაციის სიმულაცია.
    /// </summary>
    private async Task RegisterAndConfirmEmailAsync(string email, string password)
    {
        await Client.PostAsJsonAsync(RegisterAdminUrl, new RegistrationRequestDto
        {
            Email = email,
            Password = password
        });

        // Email confirmation-ს ბაზაში პირდაპირ ვაკეთებთ
        // (SMTP / email confirmation flow ამ ტესტებში ჩვენ არ გვინდა)
        using var db = GetDb();
        var user = db.ApplicationUsers.FirstOrDefault(u => u.Email == email);
        if (user != null)
        {
            user.EmailConfirmed = true;
            db.SaveChanges();
        }
    }

    /// <summary>Login-ი და LoginResponseDto-ის დაბრუნება.</summary>
    private async Task<LoginResponseDto> LoginUserAsync(string email, string password)
    {
        var response = await Client.PostAsJsonAsync(LoginUrl, new LoginRequestDto
        {
            UserName = email,
            Password = password
        });

        var content = await response.Content.ReadAsStringAsync();
        var parsed = JsonSerializer.Deserialize<CommonResponseWrapper<LoginResponseDto>>(content, JsonOptions);
        return parsed!.Result;
    }
}
