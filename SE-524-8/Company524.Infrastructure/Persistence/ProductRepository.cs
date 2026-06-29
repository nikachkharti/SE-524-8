using Company524.Application.Contracts.Persistence;
using Company524.Domain.Entities;
using Company524.Infrastructure.Data;
using Company524.Infrastructure.Persistence;

namespace Company524.API.Repository
{
    public class ProductRepository : RepositoryBase<Product, ApplicationDbContext>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
