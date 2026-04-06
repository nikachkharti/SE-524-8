using MiniBank.Repository.Interfaces;
using MiniBank.Service.Dtos.Account;
using MiniBank.Service.Interfaces;

namespace MiniBank.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public List<GetAccountDto> GetAccountsOfCustomer(int customerId)
        {
            var accounts = _accountRepository.GetAccountsOfCustomer(customerId);

            return accounts.Select(a => new GetAccountDto
            {
                Id = a.Id,
                Iban = a.Iban,
                Currency = a.Currency,
                Balance = a.Balance
            }).ToList();
        }
    }
}
