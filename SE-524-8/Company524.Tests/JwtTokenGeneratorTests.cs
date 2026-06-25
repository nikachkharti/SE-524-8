using Company524.API.Models.Authentication;
using Company524.API.Service;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace Company524.Tests
{
    /// <summary>
    /// Unit tests for JwtTokenGenerator.
    ///
    /// KEY CONCEPT — Testing without mocking (pure logic tests):
    /// JwtTokenGenerator has no database or network calls — it only uses
    /// IConfiguration and pure cryptographic logic. We can test it with a
    /// real in-memory configuration, making these pure logic tests.
    /// This is the simplest and most reliable kind of unit test.
    /// </summary>
    public class JwtTokenGeneratorTests
    {
        private readonly JwtTokenGenerator _sut;

        public JwtTokenGeneratorTests()
        {
            // Build an in-memory IConfiguration — no appsettings.json needed in tests
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Secret"] = "super-secret-test-key-that-is-long-enough-for-hmac512",
                    ["Jwt:Issuer"] = "TestIssuer",
                    ["Jwt:Audience"] = "TestAudience",
                    ["Jwt:AccessTokenExpiryMinutes"] = "15",
                    ["Jwt:RefreshTokenExpiryDays"] = "7"
                })
                .Build();

            _sut = new JwtTokenGenerator(config);
        }

        private static ApplicationUser SampleUser() => new()
        {
            Id = Guid.NewGuid().ToString(),
            Email = "test@example.com",
            UserName = "test@example.com"
        };

        // -----------------------------------------------------------------------
        // GenerateToken
        // -----------------------------------------------------------------------

        [Fact]
        public void GenerateToken_WhenCalled_ReturnsNonEmptyString()
        {
            var token = _sut.GenerateToken(SampleUser(), ["Admin"]);

            token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GenerateToken_WhenCalled_ReturnsValidJwtFormat()
        {
            // A JWT always has exactly 3 parts separated by dots: header.payload.signature
            var token = _sut.GenerateToken(SampleUser(), ["Admin"]);
            var parts = token.Split('.');

            parts.Should().HaveCount(3, "a JWT must have header, payload, and signature");
        }

        [Fact]
        public void GenerateToken_ContainsUserEmailClaim()
        {
            var user = SampleUser();
            var token = _sut.GenerateToken(user, ["Admin"]);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var emailClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Email);

            emailClaim.Should().NotBeNull();
            emailClaim!.Value.Should().Be(user.Email);
        }

        [Fact]
        public void GenerateToken_ContainsSubClaim_EqualToUserId()
        {
            var user = SampleUser();
            var token = _sut.GenerateToken(user, []);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var subClaim = jwt.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub);

            subClaim!.Value.Should().Be(user.Id);
        }

        [Fact]
        public void GenerateToken_ContainsRoleClaims()
        {
            var user = SampleUser();
            var token = _sut.GenerateToken(user, ["Admin", "Supplier"]);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var roleClaims = jwt.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            roleClaims.Should().Contain("Admin");
            roleClaims.Should().Contain("Supplier");
        }

        [Fact]
        public void GenerateToken_WhenNoRoles_DoesNotContainRoleClaims()
        {
            var token = _sut.GenerateToken(SampleUser(), []);

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            var roleClaims = jwt.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.Role);

            roleClaims.Should().BeEmpty();
        }

        [Fact]
        public void GenerateToken_TwoCallsForSameUser_ProduceDifferentTokens()
        {
            // Because Jti (JWT ID) is a new Guid each time, tokens must differ
            var user = SampleUser();
            var token1 = _sut.GenerateToken(user, ["Admin"]);
            var token2 = _sut.GenerateToken(user, ["Admin"]);

            token1.Should().NotBe(token2);
        }

        // -----------------------------------------------------------------------
        // GenerateRefreshToken
        // -----------------------------------------------------------------------

        [Fact]
        public void GenerateRefreshToken_WhenCalled_ReturnsNonEmptyString()
        {
            var token = _sut.GenerateRefreshToken();

            token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GenerateRefreshToken_WhenCalled_ReturnsBase64String()
        {
            var token = _sut.GenerateRefreshToken();

            // 64 random bytes → 88 base64 chars (with padding)
            var act = () => Convert.FromBase64String(token);
            act.Should().NotThrow("the token must be valid base64");
        }

        [Fact]
        public void GenerateRefreshToken_TwoCalls_ProduceUniqueTokens()
        {
            // Cryptographic randomness must produce different values each time
            var token1 = _sut.GenerateRefreshToken();
            var token2 = _sut.GenerateRefreshToken();

            token1.Should().NotBe(token2);
        }

        [Fact]
        public void GenerateRefreshToken_WhenCalled_Has88CharLength()
        {
            // 64 bytes in Base64 = 88 characters (with = padding)
            var token = _sut.GenerateRefreshToken();
            token.Length.Should().Be(88);
        }
    }
}
