using MiniBank.Repository.Interfaces;
using MiniBank.Repository.Models;
using MiniBank.Repository.Models.Enums;
using System.Text;

namespace MiniBank.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private const string _filePath = @"../../../../MiniBank.Data/Customers.csv";
        private readonly List<Customer> _customers;

        public CustomerRepository()
        {
            _customers = LoadData().ToList();
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
        private static IEnumerable<Customer> LoadData()
        {
            if (!File.Exists(_filePath))
                yield break;

            using var fs = new FileStream(
                _filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096,    // 4KB
                useAsync: false
            );

            using var reader = new StreamReader(fs);

            bool headerSkipped = false;

            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (!headerSkipped)
                {
                    headerSkipped = true;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var customer = FromCsv(line);
                if (customer != null)
                    yield return customer;
            }
        }
        private void SaveData()
        {
            using var fs = new FileStream(
                _filePath,
                FileMode.Create,
                FileAccess.Write,
                FileShare.Write,
                bufferSize: 4096,    // 4KB
                useAsync: false
            );

            using var writer = new StreamWriter(fs, Encoding.UTF8);

            //header
            writer.WriteLine("Id,Name,IdentityNumber,PhoneNumber,Email,CustomerType");

            //write rows
            foreach (var customer in _customers)
                writer.WriteLine(ToCsv(customer));

            writer.Flush();
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
                CustomerType = Enum.Parse<CustomerType>(parts[5])
            };

        }
        private static string ToCsv(Customer customer) => $"{customer.Id},{customer.Name},{customer.IdentityNumber},{customer.PhoneNumber},{customer.Email},{customer.CustomerType}";

        #endregion
    }
}
