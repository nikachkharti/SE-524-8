using Company524.Application.Contracts.Persistence;
using Company524.Domain.Entities;
using Company524.Infrastructure.Data;

namespace Company524.Infrastructure.Persistence
{
    public class ProductImageRepository : RepositoryBase<ProductImage, ApplicationDbContext>, IProductImageRepository
    {
        public ProductImageRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
