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
            //listbox _ ის ცვლილებაზე მინდა რომ აირჩიოთ შესაბამისი ტიპი და შემდეგ მისი დეტალები გამოიტანოს MessageBox-ით

            MessageBox.Show($"AQ GAMOITANET ARCHEULI USER IS INFO GAMOIYENET CHATGPT !!!");

        }
    }
}
