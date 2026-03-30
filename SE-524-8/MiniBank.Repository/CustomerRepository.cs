using MiniBank.Repository.Interfaces;
using MiniBank.Repository.Models;
using MiniBank.Repository.Models.Enums;
using System.Text;

namespace MiniBank.Repository
{
    //private const string _filePath = @"../../../../MiniBank.Data/Customers.csv";

    public class CustomerRepository : ICustomerRepository
    {
        private readonly string _filePath;
        private readonly List<Customer> _customers;
        private readonly object _lock = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        private CustomerRepository(string filePath, List<Customer> customers)
        {
            _filePath = filePath;
            _customers = customers;
        }

        /// <summary>
        /// Factory Method async constructor
        /// </summary>
        public static async Task<CustomerRepository> CreateAsync(string filePath)
        {
            var customers = new List<Customer>();

            await foreach (var customer in LoadDataAsync(filePath))
                customers.Add(customer);

            return new CustomerRepository(filePath, customers);
        }
        public List<Customer> GetCustomers()
        {
            lock (_lock)
            {
                return _customers.ToList();
            }
        }
        public Customer GetSingleCustomer(int id)
        {
            lock (_lock)
            {
                return _customers.FirstOrDefault(c => c.Id == id);
            }
        }
        public async Task<int> AddCustomerAsync(Customer newCustomer)
        {
            lock (_lock)
            {
                newCustomer.Id = _customers.Any() ? _customers.Max(c => c.Id) + 1 : 1;
                _customers.Add(newCustomer);
            }

            await SaveDataAsync();
            return newCustomer.Id;
        }
        public async Task<int> DeleteCustomerAsync(int id)
        {
            Customer customer;

            lock (_lock)
            {
                customer = _customers.FirstOrDefault(c => c.Id == id);
                if (customer == null)
                    return -1;

                _customers.Remove(customer);
            }

            await SaveDataAsync();
            return customer.Id;
        }
        public async Task<int> UpdateCustomerAsync(Customer customer)
        {
            bool updated = false;

            lock (_lock)
            {
                var index = _customers.FindIndex(c => c.Id == customer.Id);
                if (index >= 0)
                {
                    _customers[index] = customer;
                    updated = true;
                }
            }

            if (updated)
                await SaveDataAsync();

            return customer.Id;
        }

        #region HELPERS

        //წაკითხვა
        private static async IAsyncEnumerable<Customer> LoadDataAsync(string filePath)
        {
            if (!File.Exists(filePath))
                yield break;

            //FileStream კითხულობს მონაცემებს buffer - ებად, ანუ ნაწილ-ნაწილ
            using var fs = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 4096, //4096 (4 KB) buffer - ის default ზომა
                useAsync: true);

            using var reader = new StreamReader(fs);

            bool headerSkipped = false;

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (!headerSkipped)
                {
                    headerSkipped = true;
                    continue; // skip header
                }

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var customer = FromCsv(line);
                if (customer != null)
                    yield return customer;
            }
        }
        private static Customer FromCsv(string line)
        {
            var parts = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 6)
                throw new FormatException("Customer format is invalid");

            return new Customer
            {
                Id = int.Parse(parts[0]),
                Name = parts[1],
                IdentityNumber = parts[2],
                PhoneNumber = parts[3],
                Email = parts[4],
                CustomerType = Enum.Parse<CustomerType>(parts[5])
            };
        }

        //ჩაწერა
        private async Task SaveDataAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                using var fs = new FileStream(
                    _filePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    bufferSize: 4096,
                    useAsync: true);

                using var writer = new StreamWriter(fs, Encoding.UTF8);

                await writer.WriteLineAsync("Id,Name,IdentityNumber,PhoneNumber,Email,CustomerType");

                List<Customer> snapshot;
                lock (_lock)
                {
                    snapshot = _customers.ToList(); // snapshot to avoid long lock
                }

                foreach (var customer in snapshot)
                    await writer.WriteLineAsync(ToCsv(customer));

                await writer.FlushAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }
        private static string ToCsv(Customer customer) => $"{customer.Id},{customer.Name},{customer.IdentityNumber},{customer.PhoneNumber},{customer.Email},{customer.CustomerType}";
        #endregion

    }
}
