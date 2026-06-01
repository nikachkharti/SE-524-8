using Company524.API.Data;
using Company524.API.Entities;
using Company524.API.Repository.Base;
using Company524.API.Repository.Contracts;

namespace Company524.API.Repository
{
    public class SupplierRepository : RepositoryBase<Supplier, ApplicationDbContext>, ISupplierRepository
    {
        public SupplierRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
