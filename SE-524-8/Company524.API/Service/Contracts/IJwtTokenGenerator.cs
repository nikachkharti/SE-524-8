using Company524.API.Models.Authentication;

namespace Company524.API.Service.Contracts
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);

        /// <summary>
        /// Cryptographically random — not a JWT, just an opaque token
        /// </summary>
        string GenerateRefreshToken();
    }
}
