using MiniBank.Repository;
using MiniBank.Repository.Interfaces;
using MiniBank.Repository.Models;
using MiniBank.Repository.Models.Enums;

namespace MiniBank.ConsoleUI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IOperationRepository opeRepo = new OperationRepository();
            var newOpResult = opeRepo.AddOperation(new Operation()
            {
                Id = 0,
                AccountId = 1,
                Amount = 500,
                HappendAt = DateTime.Now,
                OperationType = OperationType.Debit
            });

            var singleOp = opeRepo.GetSingleOperation(1);
        }
    }
}
