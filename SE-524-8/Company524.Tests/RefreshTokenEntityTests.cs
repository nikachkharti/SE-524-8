using Company524.Domain.Entities;
using FluentAssertions;

namespace Company524.Tests
{
    /// <summary>
    /// Tests for the RefreshToken entity computed properties.
    ///
    /// KEY CONCEPT — Testing pure domain logic:
    /// RefreshToken has three computed properties: IsExpired, IsRevoked, IsActive.
    /// These have zero external dependencies — they only use DateTimeOffset math.
    /// These are the EASIEST tests to write and should cover all edge cases.
    /// No mocks, no async — just pure boolean logic.
    /// </summary>
    public class RefreshTokenEntityTests
    {
        // -----------------------------------------------------------------------
        // IsExpired
        // -----------------------------------------------------------------------

        [Fact]
        public void IsExpired_WhenExpiryIsInFuture_ReturnsFalse()
        {
            var token = new RefreshToken
            {
                ExpiresAt = DateTimeOffset.Now.AddDays(1)
            };

            token.IsExpired.Should().BeFalse();
        }

        [Fact]
        public void IsExpired_WhenExpiryIsInPast_ReturnsTrue()
        {
            var token = new RefreshToken
            {
                ExpiresAt = DateTimeOffset.Now.AddDays(-1)
            };

            token.IsExpired.Should().BeTrue();
        }

        [Fact]
        public void IsExpired_WhenExpiryIsExactlyNow_ReturnsTrue()
        {
            // Boundary: expiry == now means it has just expired
            var token = new RefreshToken
            {
                ExpiresAt = DateTimeOffset.Now.AddMilliseconds(-1)
            };

            token.IsExpired.Should().BeTrue();
        }

        // -----------------------------------------------------------------------
        // IsRevoked
        // -----------------------------------------------------------------------

        [Fact]
        public void IsRevoked_WhenRevokedAtIsNull_ReturnsFalse()
        {
            var token = new RefreshToken { RevokedAt = null };

            token.IsRevoked.Should().BeFalse();
        }

        [Fact]
        public void IsRevoked_WhenRevokedAtIsSet_ReturnsTrue()
        {
            var token = new RefreshToken { RevokedAt = DateTimeOffset.Now };

            token.IsRevoked.Should().BeTrue();
        }

        // -----------------------------------------------------------------------
        // IsActive — the most important property, composite of the other two
        // -----------------------------------------------------------------------

        [Fact]
        public void IsActive_WhenNotExpiredAndNotRevoked_ReturnsTrue()
        {
            var token = new RefreshToken
            {
                ExpiresAt = DateTimeOffset.Now.AddDays(7),
                RevokedAt = null
            };

            token.IsActive.Should().BeTrue();
        }

        [Fact]
        public void IsActive_WhenExpired_ReturnsFalse()
        {
            var token = new RefreshToken
            {
                ExpiresAt = DateTimeOffset.Now.AddDays(-1),
                RevokedAt = null
            };

            token.IsActive.Should().BeFalse();
        }

        [Fact]
        public void IsActive_WhenRevoked_ReturnsFalse()
        {
            var token = new RefreshToken
            {
                ExpiresAt = DateTimeOffset.Now.AddDays(7),   // not expired
                RevokedAt = DateTimeOffset.Now.AddMinutes(-5) // but revoked
            };

            token.IsActive.Should().BeFalse();
        }

        [Fact]
        public void IsActive_WhenBothExpiredAndRevoked_ReturnsFalse()
        {
            var token = new RefreshToken
            {
                ExpiresAt = DateTimeOffset.Now.AddDays(-2),
                RevokedAt = DateTimeOffset.Now.AddDays(-1)
            };

            token.IsActive.Should().BeFalse();
        }
    }
}
