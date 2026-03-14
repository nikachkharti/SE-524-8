using MiniBank.Repository.Interfaces;
using MiniBank.Repository.Models;

namespace MiniBank.Repository
{
    public class OperationRepository : IOperationRepository
    {
        //1. გადარიცხვა, შეტანა, გამოტანა

        public Operation GetSingleOperation(int operationId)
        {
            throw new NotImplementedException();
        }

        public List<Operation> GetOperationsOfAccount(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
