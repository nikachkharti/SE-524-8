using Company524.Application.Contracts.Persistence;
using Company524.Domain.Entities;
using Company524.Infrastructure.Data;

namespace Company524.Infrastructure.Persistence
{
    public class OrderRepository : RepositoryBase<Order, ApplicationDbContext>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
