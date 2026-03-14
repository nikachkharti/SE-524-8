using MiniBank.Repository.Models;

namespace MiniBank.Repository.Interfaces
{
    public interface ICustomerRepository
    {
        int AddCustomer(Customer newCustomer);
        Customer GetCustomer(int id);
        List<Customer> GetCustomers();
        int UpdateCustomer(Customer customer);
        int DeleteCustomer(int id);
    }
}
