using MiniBank.Repository;
using MiniBank.Repository.Models;
using MiniBank.Repository.Models.Enums;

namespace MiniBank.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomerRepository customerRepository = new CustomerRepository();
            var singleCustomer = customerRepository.GetCustomer(9);
            var allCustomers = customerRepository.GetCustomers();

            var addResult = customerRepository.AddCustomer(new Customer()
            {
                Id = 0,
                Name = "Nika Chkhartishvili",
                Email = "nika@gmail.com",
                IdentityNumber = "12345678945",
                PhoneNumber = "558774499",
                CustomerType = CustomerType.Phyisical
            });


            var updateResult = customerRepository.UpdateCustomer(new Customer()
            {
                Id = 11,
                Name = "Giorgi Chkhartishvili",
                Email = "nika@gmail.com",
                IdentityNumber = "12345678945",
                PhoneNumber = "558774499",
                CustomerType = CustomerType.Phyisical
            });


            _ = customerRepository.DeleteCustomer(11);


        }
    }
}
