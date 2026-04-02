using MiniBank.Repository.Models.Enums;

namespace MiniBank.Service.Dtos.Customer
{
    public class UpdateCustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IdentityNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public CustomerType CustomerType { get; set; }

    }
}
