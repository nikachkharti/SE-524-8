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
            newCustomer.Id = _customers.
                Any()
                ? _customers.Max(c => c.Id) + 1
                : 1;

            _customers.Add(newCustomer);
            SaveData();

            return newCustomer.Id;
        }
        public Customer GetCustomer(int id) => _customers.FirstOrDefault(c => c.Id == id);
        public List<Customer> GetCustomers() => _customers;
        public int UpdateCustomer(Customer customer)
        {
            var index = _customers.FindIndex(c => c.Id == customer.Id);

            if (index >= 0)
            {
                _customers[index] = customer;
                SaveData();
            }

            return customer.Id;
        }
        public int DeleteCustomer(int id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);

            if (customer != null)
            {
                _customers.Remove(customer);
                SaveData();

                return customer.Id;
            }

            return -1;
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
        private void SaveData()
        {
            var lines = new List<string>() { "Id,Name,IdentityNumber,PhoneNumber,Email,CustomerType" };
            lines.AddRange(_customers.Select(ToCsv));
            File.WriteAllLines(_filePath, lines);
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
        private static string ToCsv(Customer customer) => $"{customer.Id},{customer.Name},{customer.IdentityNumber},{customer.PhoneNumber},{customer.Email},{customer.CustomerType}";

        #endregion
    }
}
