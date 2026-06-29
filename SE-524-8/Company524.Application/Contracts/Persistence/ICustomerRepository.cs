using Company524.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Company524.Application.Contracts.Persistence
{
    public interface ICustomerRepository : IRepositoryBase<Customer, DbContext>
    {
    }
}
