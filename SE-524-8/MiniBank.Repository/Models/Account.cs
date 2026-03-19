using MiniBank.Repository.Attributes;

namespace MiniBank.Repository.Models
{
    public class Account
    {
        [CustomRequired]
        [CustomPositive]
        public int Id { get; set; }

        [CustomRequired]
        [CustomExactLength(11)]
        public string Iban { get; set; }

        [CustomRequired]
        [CustomExactLength(3)]
        [CustomToUpper]
        public string Currency { get; set; }
        public decimal Balance { get; set; }

        [CustomRequired]
        public int CustomerId { get; set; }
        public string Destination { get; set; }
    }
}
