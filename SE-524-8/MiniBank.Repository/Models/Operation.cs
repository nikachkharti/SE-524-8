using MiniBank.Repository.Models.Enums;

namespace MiniBank.Repository.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public OperationType OperationType { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime HappendAt { get; set; } = DateTime.Now;
    }
}
