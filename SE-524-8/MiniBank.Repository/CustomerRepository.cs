using MiniBank.Repository.Models;

namespace MiniBank.Repository
{
    public class CustomerRepository
    {
        private const string _filePath = @"../../../../MiniBank.Data/Customers.csv";
        private readonly List<Customer> _customers;

        public CustomerRepository()
        {
            _customers = LoadData(_filePath);
        }


        public int AddCustomer(Customer newCustomer)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int id) => _customers.FirstOrDefault(c => c.Id == id);

        public List<Customer> GetCustomers() => _customers;


        public int UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        public int DeleteCustomer(int id)
        {
            throw new NotImplementedException();
        }


        #region HELPERS

        private static List<Customer> LoadData(string filePath)
        {
            var customers = new List<Customer>();

            if (!File.Exists(filePath))
                return customers;

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var customer = FromCsv(line);
                if (customer != null)
                    customers.Add(customer);
            }

            return customers;
        }
        private static Customer FromCsv(string line)
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 6)
                throw new FormatException("Customer format is invalid");

            return new Customer()
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                IdentityNumber = parts[2],
                PhoneNumber = parts[3],
                Email = parts[4],
                CustomerType = int.Parse(parts[5])
            };

        }




        #endregion
    }
}
