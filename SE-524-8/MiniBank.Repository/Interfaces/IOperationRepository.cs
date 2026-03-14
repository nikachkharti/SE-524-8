using MiniBank.Repository.Models;

namespace MiniBank.Repository.Interfaces
{
    public interface IOperationRepository
    {
        Operation GetSingleOperation(int operationId);
        List<Operation> GetOperationsOfAccount(int accountId);
    }
}
