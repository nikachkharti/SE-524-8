using MiniBank.Repository.Interfaces;
using MiniBank.Repository.Models;
using MiniBank.Service.Dtos.Customer;
using MiniBank.Service.Interfaces;

namespace MiniBank.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<GetCustomerDto>> GetAllCustomersAsync()
        {
            var customers = _repository.GetCustomers();

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
        public async Task<int> AddCustomerAsync(CreateCustomerDto model)
        {
            var newCustomer = new Customer
            {
                Name = model.Name,
                IdentityNumber = model.IdentityNumber,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CustomerType = model.CustomerType
            };

            return await _repository.AddCustomerAsync(newCustomer);
        }
        public async Task<int> UpdateCustomerAsync(UpdateCustomerDto model)
        {
            var customer = new Customer
            {
                Id = model.Id,
                Name = model.Name,
                IdentityNumber = model.IdentityNumber,
                PhoneNumber = model.PhoneNumber,
                Email = model.Email,
                CustomerType = model.CustomerType
            };

            return await _repository.UpdateCustomerAsync(customer);
        }
        public async Task<int> DeleteCustomerAsync(int id)
        {
            return await _repository.DeleteCustomerAsync(id);
        }
    }
}
