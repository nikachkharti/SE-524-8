using Company524.API.Data;
using Company524.API.Entities;
using Company524.API.Repository.Base;

namespace Company524.API.Repository.Contracts
{
    public interface IOrderRepository : IRepositoryBase<Order, ApplicationDbContext>
    {
    }
}
