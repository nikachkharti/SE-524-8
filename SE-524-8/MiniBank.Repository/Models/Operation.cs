using MiniBank.Repository.Models.Enums;

namespace MiniBank.Repository.Models
{
    public class Operation
    {
        //სავალდებულო
        //დადებითი
        public int Id { get; set; }
        public OperationType OperationType { get; set; }

        //სავალდებულო
        public int AccountId { get; set; }

        //სავალდებულო
        public decimal Amount { get; set; }
        public DateTime HappendAt { get; set; } = DateTime.Now;
    }
}
