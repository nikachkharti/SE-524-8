using MiniBank.Repository;
using MiniBank.Repository.Interfaces;
using MiniBank.Service.Dtos.Customer;
using MiniBank.Service.Interfaces;

namespace MiniBank.Service
{
    public class CustomerService : ICustomerService
    {
        private const string _filePath = @"../../../../MiniBank.Data/Customers.csv";

        public Task<int> AddCustomerAsync(CreateCustomerDto model)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<GetCustomerDto>> GetAllCustomersAsync()
        {
            var repository = await CustomerRepository.CreateAsync(_filePath);
            var customers = repository.GetCustomers();
            return customers.Select(c => new GetCustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                IdentityNumber = c.IdentityNumber,
                PhoneNumber = c.PhoneNumber,
                Email = c.Email,
                CustomerType = c.CustomerType
            }).ToList();
        }

        public Task<GetCustomerDto> GetSingleCustomerAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateCustomerAsync(UpdateCustomerDto model)
        {
            throw new NotImplementedException();
        }
    }
}
