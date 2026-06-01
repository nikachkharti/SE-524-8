using Company524.API.Data;
using Company524.API.Entities;
using Company524.API.Repository.Base;
using Company524.API.Repository.Contracts;

namespace Company524.API.Repository
{
    public class CustomerRepository : RepositoryBase<Customer, ApplicationDbContext>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
