using MiniBank.Repository.Models.Enums;
using MiniBank.Service.Dtos.Customer;

namespace MiniBank.Service.Interfaces
{
    public interface ICustomerService
    {
        Task<List<GetCustomerDto>> GetAllCustomersAsync();
        Task<GetCustomerDto> GetSingleCustomerAsync(int id);
        Task<int> AddCustomerAsync(CreateCustomerDto model);
        Task<int> DeleteCustomerAsync(int id);
        Task<int> UpdateCustomerAsync(UpdateCustomerDto model);
    }
}
