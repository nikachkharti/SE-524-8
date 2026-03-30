using MiniBank.Repository.Interfaces;
using MiniBank.Repository.Models;
using MiniBank.Repository.Models.Enums;
using System.Xml.Linq;

namespace MiniBank.Repository
{
    //private const string _filePath = @"../../../../MiniBank.Data/Operations.xml";

    public class OperationRepository : IOperationRepository
    {
        private readonly string _filePath;
        private readonly List<Operation> _operations;
        private readonly object _lock = new();
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        private OperationRepository(string filePath, List<Operation> operations)
        {
            _filePath = filePath;
            _operations = operations;
        }

        /// <summary>
        /// Factory Method async constructor
        /// </summary>
        public static async Task<OperationRepository> CreateAsync(string filePath)
        {
            var operations = new List<Operation>();

            await foreach (var op in LoadDataAsync(filePath))
            {
                operations.Add(op);
            }

            return new OperationRepository(filePath, operations);
        }

        public List<Operation> GetOperationsOfAccount(int accountId)
        {
            lock (_lock)
            {
                return _operations
                    .Where(o => o.AccountId == accountId)
                    .ToList();
            }
        }
        public Operation GetSingleOperation(int operationId)
        {
            lock (_lock)
            {
                return _operations.FirstOrDefault(o => o.Id == operationId);
            }
        }
        public async Task<int> AddOperationAsync(Operation operation)
        {
            lock (_lock)
            {
                operation.Id = _operations.Any() ? _operations.Max(o => o.Id) + 1 : 1;
                _operations.Add(operation);
            }

            await SaveDataAsync();
            return operation.Id;
        }

        #region HELPERS

        /// <summary>
        /// Streams operations from XML using IAsyncEnumerable.
        /// </summary>
        private static async IAsyncEnumerable<Operation> LoadDataAsync(string filePath)
        {
            if (!File.Exists(filePath))
                yield break;

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, useAsync: true);
            using var ms = new MemoryStream();
            await fs.CopyToAsync(ms);
            ms.Position = 0;

            XDocument xdoc;
            try
            {
                xdoc = XDocument.Load(ms);
            }
            catch
            {
                yield break; // malformed XML
            }

            foreach (var el in xdoc.Root?.Elements("Operation") ?? Enumerable.Empty<XElement>())
            {
                var operation = new Operation
                {
                    Id = (int)el.Element("Id")!,
                    OperationType = Enum.Parse<OperationType>((string)el.Element("OperationType")),
                    AccountId = (int)el.Element("AccountId"),
                    Amount = (decimal)el.Element("Amount"),
                    HappendAt = (DateTime)el.Element("HappendAt")
                };

                yield return operation;
            }
        }

        /// <summary>
        /// Saves the in-memory list of operations to XML.
        /// </summary>
        private async Task SaveDataAsync()
        {
            await _semaphore.WaitAsync();
            try
            {
                List<Operation> snapshot;

                lock (_lock)
                {
                    snapshot = _operations.ToList(); // avoid long lock
                }

                var xdoc = new XDocument(
                    new XElement("Operations",
                        snapshot.Select(o =>
                            new XElement("Operation",
                                new XElement("Id", o.Id),
                                new XElement("OperationType", o.OperationType),
                                new XElement("AccountId", o.AccountId),
                                new XElement("Amount", o.Amount),
                                new XElement("HappendAt", o.HappendAt)
                            ))
                    )
                );

                using var ms = new MemoryStream();
                xdoc.Save(ms);
                ms.Position = 0;

                using var fs = new FileStream(
                    _filePath,
                    FileMode.Create,
                    FileAccess.Write,
                    FileShare.None,
                    8192,
                    useAsync: true);

                await ms.CopyToAsync(fs);
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
