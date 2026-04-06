using MiniBank.Service.Dtos.Account;

namespace MiniBank.Service.Interfaces
{
    public interface IAccountService
    {
        List<GetAccountDto> GetAccountsOfCustomer(int customerId);
    }
}
