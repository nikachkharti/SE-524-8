using MiniBank.Repository;
using MiniBank.Repository.Interfaces;
using MiniBank.Service.Dtos.Customer;
using MiniBank.Service.Interfaces;

namespace MiniBank.Service
{
    public class CustomerService : ICustomerService
    {
        private const string _filePath = @"../../../../MiniBank.Data/Customers.csv";

        public async Task<int> AddCustomerAsync(CreateCustomerDto model)
        {
            var repository = await CustomerRepository.CreateAsync(_filePath);
            var newCustomer = new Repository.Models.Customer
            {
                Name = model.Name,
                IdentityNumber = model.IdentityNumber,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CustomerType = model.CustomerType
            };
            return await repository.AddCustomerAsync(newCustomer);
        }

        public async Task<int> DeleteCustomerAsync(int id)
        {
            var repository = await CustomerRepository.CreateAsync(_filePath);
            return await repository.DeleteCustomerAsync(id);
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

        public async Task<int> UpdateCustomerAsync(UpdateCustomerDto model)
        {
            var repository = await CustomerRepository.CreateAsync(_filePath);
            var updatedCustomer = new Repository.Models.Customer
            {
                Id = model.Id,
                Name = model.Name,
                IdentityNumber = model.IdentityNumber,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CustomerType = model.CustomerType
            };
            return await repository.UpdateCustomerAsync(updatedCustomer);
        }
    }
}
