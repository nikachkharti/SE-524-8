using MiniBank.Repository.Attributes;
using MiniBank.Repository.Models.Enums;

namespace MiniBank.Repository.Models
{
    public class Customer
    {
        [CustomRequired]
        [CustomPositive]
        public int Id { get; set; }

        [CustomRequired]
        [CustomMaxLength(50)]
        public string Name { get; set; }

        [CustomRequired]
        [CustomExactLength(11)]
        public string IdentityNumber { get; set; }

        [CustomRequired]
        [CustomExactLength(9)]
        public string PhoneNumber { get; set; }

        [CustomRequired]
        [CustomEmail]
        public string Email { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
