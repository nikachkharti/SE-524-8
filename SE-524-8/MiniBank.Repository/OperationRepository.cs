using MiniBank.Repository.Models;

namespace MiniBank.Repository
{
    public class OperationRepository
    {
        //1. გადარიცხვა, შეტანა, გამოტანა

        Operation GetSingleOperation(int operationId)
        {
            throw new NotImplementedException();
        }

        public List<Operation> GetOperationsOfAccount(int accountId)
        {
            throw new NotImplementedException();
        }
    }
}
