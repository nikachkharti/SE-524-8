using MiniBank.Repository.Attributes;
using MiniBank.Repository.Models.Enums;

namespace MiniBank.Repository.Models
{
    public class Operation
    {
        [CustomRequired]
        [CustomPositive]
        public int Id { get; set; }
        public OperationType OperationType { get; set; }

        [CustomRequired]
        public int AccountId { get; set; }

        [CustomRequired]
        public decimal Amount { get; set; }
        public DateTime HappendAt { get; set; } = DateTime.Now;
    }
}
