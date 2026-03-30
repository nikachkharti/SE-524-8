using MiniBank.Repository.Interfaces;
using MiniBank.Repository.Models;
using System.Text;
using System.Text.Json;

namespace MiniBank.Repository
{
    //private const string _filePath = @"../../../../MiniBank.Data/Accounts.json";
    public class AccountRepository : IAccountRepository
    {
        private readonly string _filePath;
        private readonly List<Account> _accounts;
        private readonly object _lock = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private AccountRepository(string filePath, List<Account> accounts)
        {
            _filePath = filePath;
            _accounts = accounts;
        }

        /// <summary>
        /// Async factory method
        /// </summary>
        public static async Task<AccountRepository> CreateAsync(string filePath)
        {
            var accounts = new List<Account>();

            await foreach (var acc in LoadDataAsync(filePath))
            {
                accounts.Add(acc);
            }

            return new AccountRepository(filePath, accounts);
        }
        public List<Account> GetAccounts()
        {
            lock (_lock)
            {
                return _accounts.ToList();
            }
        }
        public Account GetSingleAccount(int id)
        {
            lock (_lock)
            {
                return _accounts.FirstOrDefault(a => a.Id == id);
            }
        }
        public List<Account> GetAccountsOfCustomer(int customerId)
        {
            lock (_lock)
            {
                return _accounts
                    .Where(a => a.CustomerId == customerId)
                    .ToList();
            }
        }
        public async Task<int> AddAccountAsync(Account newAccount)
        {
            lock (_lock)
            {
                newAccount.Id = _accounts.Any() ? _accounts.Max(a => a.Id) + 1 : 1;
                _accounts.Add(newAccount);
            }

            await SaveDataAsync();
            return newAccount.Id;
        }
        public async Task<int> DeleteAccountAsync(int id)
        {
            Account account;

            lock (_lock)
            {
                account = _accounts.FirstOrDefault(a => a.Id == id);
                if (account == null)
                    return -1;

                _accounts.Remove(account);
            }

            await SaveDataAsync();
            return account.Id;
        }
        public async Task<int> UpdateAccountAsync(Account account)
        {
            bool updated = false;

            lock (_lock)
            {
                var index = _accounts.FindIndex(a => a.Id == account.Id);
                if (index >= 0)
                {
                    _accounts[index] = account;
                    updated = true;
                }
            }

            if (updated)
                await SaveDataAsync();

            return account.Id;
        }


        #region HELPERS

        public static async IAsyncEnumerable<Account> LoadDataAsync(string filePath)
        {
            if (!File.Exists(filePath))
                yield break;

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, useAsync: true);
            using var ms = new MemoryStream();
            await fs.CopyToAsync(ms);
            ms.Position = 0;

            var json = Encoding.UTF8.GetString(ms.ToArray());

            List<Account> deserialized = null;
            try
            {
                deserialized = JsonSerializer.Deserialize<List<Account>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch
            {
                yield break; // invalid JSON
            }

            if (deserialized == null) yield break;

            foreach (var account in deserialized)
            {
                yield return account;
            }
        }
        private async Task SaveDataAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                List<Account> snapshot;

                lock (_lock)
                {
                    snapshot = _accounts.ToList(); // avoid long lock
                }

                var jsonPayload = JsonSerializer.Serialize(snapshot, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                using var fs = new FileStream(
                    _filePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    8192,
                    useAsync: true);

                var bytes = Encoding.UTF8.GetBytes(jsonPayload);
                await fs.WriteAsync(bytes, 0, bytes.Length);
                await fs.FlushAsync();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        #endregion
    }
}
