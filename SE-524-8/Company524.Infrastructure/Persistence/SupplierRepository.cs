using Company524.Application.Contracts.Persistence;
using Company524.Domain.Entities;
using Company524.Infrastructure.Data;

namespace Company524.Infrastructure.Persistence
{
    public class SupplierRepository : RepositoryBase<Supplier, ApplicationDbContext>, ISupplierRepository
    {
        public SupplierRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
