using Company524.API.Models.Authentication;
using Company524.API.Service.Contracts;

namespace Company524.API.Service
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        public string GenerateToken(ApplicationUser applicationUser, IEnumerable<string> roles)
        {
            throw new NotImplementedException();
        }
    }
}
