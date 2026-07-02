using Company524.Application.Contracts.Persistence;
using Company524.Domain.Entities;
using Company524.Infrastructure.Data;

namespace Company524.Infrastructure.Persistence
{
    public class RefreshTokenRepository : RepositoryBase<RefreshToken, ApplicationDbContext>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
