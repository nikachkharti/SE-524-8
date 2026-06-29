using Company524.Domain.Entities;

namespace Company524.Application.Contracts.Service
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles);
        string GenerateRefreshToken();
    }
}
