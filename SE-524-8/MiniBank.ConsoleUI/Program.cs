using MiniBank.Repository;

namespace MiniBank.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CustomerRepository customerRepository = new CustomerRepository();
            var singleCustomer = customerRepository.GetCustomer(9);
            var allCustomers = customerRepository.GetCustomers();

            var addResult = customerRepository.AddCustomer(new Repository.Models.Customer()
            {
                Id = 30,
                Name = "Nika Chkhartishvili",
                Email = "nika@gmail.com",
                IdentityNumber = "12345678945",
                PhoneNumber = "558774499",
                CustomerType = 0 //PHYISICAL PERSON
            });

        }
    }
}
