using MiniBank.Repository.Models;

namespace MiniBank.Repository.Interfaces
{
    public interface IAccountRepository
    {
        List<Account> GetAccounts();
        List<Account> GetAccountsOfCustomer(int customerId);
        Account GetSingleAccount(int id);
        Task<int> AddAccountAsync(Account newAccount);
        Task<int> UpdateAccountAsync(Account account);
        Task<int> DeleteAccountAsync(int id);
    }
}
