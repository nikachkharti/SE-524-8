using MiniBank.Repository.Interfaces;
using MiniBank.Repository.Models;
using System.Text;
using System.Text.Json;

namespace MiniBank.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _filePath;
        private readonly List<Account> _accounts;

        private AccountRepository(string filePath, List<Account> accounts)
        {
            _filePath = filePath;
            _accounts = accounts;
        }

        /// <summary>
        /// Factory method
        /// </summary>
        public static AccountRepository Create(string filePath)
        {
            var accounts = new List<Account>();

            foreach (var acc in LoadData(filePath))
            {
                accounts.Add(acc);
            }

            return new AccountRepository(filePath, accounts);
        }

        public List<Account> GetAccounts() => _accounts;

        public Account GetSingleAccount(int id)
            => _accounts.FirstOrDefault(a => a.Id == id);

        public List<Account> GetAccountsOfCustomer(int customerId)
            => _accounts.Where(a => a.CustomerId == customerId).ToList();

        public int AddAccount(Account newAccount)
        {
            newAccount.Id = _accounts.Any() ? _accounts.Max(a => a.Id) + 1 : 1;

            _accounts.Add(newAccount);
            SaveData();

            return newAccount.Id;
        }

        public int DeleteAccount(int id)
        {
            var account = _accounts.FirstOrDefault(a => a.Id == id);
            if (account == null) return -1;

            _accounts.Remove(account);
            SaveData();

            return account.Id;
        }

        public int UpdateAccount(Account account)
        {
            var index = _accounts.FindIndex(a => a.Id == account.Id);

            if (index >= 0)
            {
                _accounts[index] = account;
                SaveData();
            }

            return account.Id;
        }

        #region HELPERS

        public static IEnumerable<Account> LoadData(string filePath)
        {
            if (!File.Exists(filePath))
                yield break;

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192);
            using var ms = new MemoryStream();

            fs.CopyTo(ms); // synchronous copy
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
                yield break;
            }

            if (deserialized == null)
                yield break;

            foreach (var account in deserialized)
            {
                yield return account;
            }
        }

        private void SaveData()
        {
            var jsonPayload = JsonSerializer.Serialize(
                _accounts,
                new JsonSerializerOptions { WriteIndented = true });

            using var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192);

            var bytes = Encoding.UTF8.GetBytes(jsonPayload);

            fs.Write(bytes, 0, bytes.Length);
            fs.Flush();
        }

        #endregion

    }
}
