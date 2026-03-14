using MiniBank.Repository.Models;

namespace MiniBank.Repository.Interfaces
{
    public interface IOperationRepository
    {
        int AddOperation(Operation operation);
        Operation GetSingleOperation(int operationId);
        List<Operation> GetOperationsOfAccount(int accountId);
    }
}
