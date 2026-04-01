using MiniBank.Service.Interfaces;

namespace MiniBank.UI
{
    public partial class Form1 : Form
    {
        private readonly ICustomerService _customerService;

        public Form1()
        {
            InitializeComponent();
            _customerService = new Service.CustomerService();
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

            //MessageBox.Show($"Selected Customer: {selectedCustomer.Name}\nIdentity Number: {selectedCustomer.IdentityNumber}\nPhone: {selectedCustomer.PhoneNumber}\nEmail: {selectedCustomer.Email}\nCustomer Type: {selectedCustomer.CustomerType}");
        }
    }
}
