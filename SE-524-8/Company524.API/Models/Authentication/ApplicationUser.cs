using Company524.API.Entities;
using Microsoft.AspNetCore.Identity;

namespace Company524.API.Models.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
