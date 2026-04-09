using MiniBank.Service.Dtos.Account;
using MiniBank.Service.Interfaces;

namespace MiniBank.UI
{
    public partial class AccountsUC : UserControl
    {
        private readonly IAccountService _accountService;
        private readonly int _customerId;
        public AccountsUC(IAccountService accountService, int customerId)
        {
            InitializeComponent();
            _accountService = accountService;
            _customerId = customerId;
        }

        private void AccountsUC_Load(object sender, EventArgs e)
        {
            var accounts = LoadAccountsOfCustomer(_customerId);
            accountListBox.DataSource = accounts;
        }

        private List<GetAccountDto> LoadAccountsOfCustomer(int customerId)
        {
            return _accountService.GetAccountsOfCustomer(customerId);
        }


    }
}
