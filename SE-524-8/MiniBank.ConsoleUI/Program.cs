using MiniBank.Repository.Models;
using MiniBank.Repository.Models.Enums;
using MiniBank.Repository.Validators;

namespace MiniBank.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Customer newCustomer = new();
            newCustomer.Id = -1;
            newCustomer.Name = string.Empty;
            newCustomer.Email = "invalid-email";
            newCustomer.IdentityNumber = "123";
            newCustomer.PhoneNumber = "123";
            newCustomer.CustomerType = CustomerType.Phyisical;



            Validator.Validate(newCustomer);
        }
    }
}
