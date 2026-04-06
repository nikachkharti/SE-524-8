using MiniBank.Repository.Models.Enums;
using MiniBank.Service.Dtos.Account;
using MiniBank.Service.Dtos.Customer;
using MiniBank.Service.Interfaces;

namespace MiniBank.UI
{
    public partial class Form1 : Form
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;

        public Form1(ICustomerService customerService, IAccountService accountService)
        {
            InitializeComponent();
            _customerService = customerService;
            _accountService = accountService;
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadCustomersAsync();
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCustomer = listBox1.SelectedItem as GetCustomerDto;
            if (selectedCustomer != null)
            {
                nameValue.Text = selectedCustomer.Name;
                idValue.Text = selectedCustomer.IdentityNumber;
                phoneNumberValue.Text = selectedCustomer.PhoneNumber;
                emailValue.Text = selectedCustomer.Email;
                customerTypeCombo.DataSource = Enum.GetValues(typeof(CustomerType));
                customerTypeCombo.SelectedItem = selectedCustomer.CustomerType;

                var accounts = LoadAccountsOfCustomer(selectedCustomer.Id);
                accountListBox.DataSource = accounts;
            }
        }


        private async Task LoadCustomersAsync()
        {
            var customers = await _customerService.GetAllCustomersAsync();
            listBox1.DataSource = customers;
            listBox1.DisplayMember = "Name";
        }

        private List<GetAccountDto> LoadAccountsOfCustomer(int customerId)
        {
            return _accountService.GetAccountsOfCustomer(customerId);
        }

        private async void newCustomerBtn_Click(object sender, EventArgs e)
        {
            var createCustomerDto = new CreateCustomerDto()
            {
                Name = nameValue.Text,
                IdentityNumber = idValue.Text,
                PhoneNumber = phoneNumberValue.Text,
                Email = emailValue.Text,
                CustomerType = (CustomerType)customerTypeCombo.SelectedItem
            };

            await _customerService.AddCustomerAsync(createCustomerDto);
            await LoadCustomersAsync();
        }

        private async void deleteCustomerBtn_Click_1(object sender, EventArgs e)
        {
            var selectedCustomer = listBox1.SelectedItem as Service.Dtos.Customer.GetCustomerDto;
            if (selectedCustomer != null)
            {
                await _customerService.DeleteCustomerAsync(selectedCustomer.Id);
                await LoadCustomersAsync();
            }
        }

        private async void updateCustomerBtn_Click_1(object sender, EventArgs e)
        {
            var selectedCustomer = listBox1.SelectedItem as Service.Dtos.Customer.GetCustomerDto;
            if (selectedCustomer != null)
            {
                var updateCustomerDto = new UpdateCustomerDto()
                {
                    Id = selectedCustomer.Id,
                    Name = nameValue.Text,
                    IdentityNumber = idValue.Text,
                    PhoneNumber = phoneNumberValue.Text,
                    Email = emailValue.Text,
                    CustomerType = (CustomerType)customerTypeCombo.SelectedItem
                };
                await _customerService.UpdateCustomerAsync(updateCustomerDto);
                await LoadCustomersAsync();
            }
        }

        private void clearFormBtn_Click_1(object sender, EventArgs e)
        {
            nameValue.Text = string.Empty;
            phoneNumberValue.Text = string.Empty;
            idValue.Text = string.Empty;
            emailValue.Text = string.Empty;
            customerTypeCombo.SelectedIndex = 0;
        }

        private void openAccountBtn_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }
    }
}
