using MiniBank.Repository.Models.Enums;
using MiniBank.Service.Interfaces;

namespace MiniBank.UI
{
    public partial class Form1 : Form
    {
        private readonly ICustomerService _customerService;

        public Form1(ICustomerService customerService)
        {
            InitializeComponent();
            _customerService = customerService;
        }


        private async void Form1_Load(object sender, EventArgs e)
        {
            var customers = await _customerService.GetAllCustomersAsync();
            listBox1.DataSource = customers;
            listBox1.DisplayMember = "Name";
        }

        private async void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCustomer = listBox1.SelectedItem as Service.Dtos.Customer.GetCustomerDto;
            if (selectedCustomer != null)
            {
                nameValue.Text = selectedCustomer.Name;
                idValue.Text = selectedCustomer.IdentityNumber;
                phoneNumberValue.Text = selectedCustomer.PhoneNumber;
                emailValue.Text = selectedCustomer.Email;
            }
        }
    }
}
