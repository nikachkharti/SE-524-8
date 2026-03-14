using MiniBank.Repository.Models;

namespace MiniBank.Repository.Interfaces
{
    public interface IAccountRepository
    {
        public List<Account> GetAccounts();
        Account GetSingleAccount(int id);
        List<Account> GetAccountsOfCustomer(int customerId);
        int AddAccount(Account newAccount);
        int DeleteAccount(int id);
        int UpdateAccount(Account account);
    }
}
