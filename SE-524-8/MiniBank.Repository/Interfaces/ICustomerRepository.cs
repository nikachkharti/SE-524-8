using MiniBank.Repository.Models;

namespace MiniBank.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        List<Customer> GetCustomers();
        Customer GetSingleCustomer(int id);
        Task<int> AddCustomerAsync(Customer newCustomer);
        Task<int> UpdateCustomerAsync(Customer customer);
        Task<int> DeleteCustomerAsync(int id);
    }
}
