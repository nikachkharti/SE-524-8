using MiniBank.Repository.Models;

namespace MiniBank.Repository.Interfaces
{
    public interface IOperationRepository
    {
        Task<int> AddOperationAsync(Operation operation);
        Operation GetSingleOperation(int operationId);
        List<Operation> GetOperationsOfAccount(int accountId);
    }
}
