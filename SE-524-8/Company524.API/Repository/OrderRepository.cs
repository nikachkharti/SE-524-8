using Company524.API.Data;
using Company524.API.Entities;
using Company524.API.Repository.Base;
using Company524.API.Repository.Contracts;

namespace Company524.API.Repository
{
    public class OrderRepository : RepositoryBase<Order, ApplicationDbContext>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
