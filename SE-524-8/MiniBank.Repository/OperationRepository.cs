using MiniBank.Repository.Interfaces;
using MiniBank.Repository.Models;
using MiniBank.Repository.Models.Enums;
using System.Xml.Linq;

namespace MiniBank.Repository
{
    public class OperationRepository : IOperationRepository
    {
        private const string _filePath = @"../../../../MiniBank.Data/Operations.xml";
        private readonly List<Operation> _operations;

        public OperationRepository()
        {
            _operations = LoadData(_filePath).ToList();
        }

        public List<Operation> GetOperationsOfAccount(int accountId) =>
            _operations
                .Where(o => o.AccountId == accountId)
            .ToList();

        public Operation GetSingleOperation(int operationId) =>
            _operations.FirstOrDefault(o => o.Id == operationId);

        public int AddOperation(Operation operation)
        {
            operation.Id = _operations.Any() ? _operations.Max(o => o.Id) + 1 : 1;
            _operations.Add(operation);
            SaveData();
            return operation.Id;
        }


        #region HELPERS
        private IEnumerable<Operation> LoadData(string filePath)
        {
            if (!File.Exists(filePath))
                yield break;

            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 8192, useAsync: true);
            using var ms = new MemoryStream();
            fs.CopyTo(ms);
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
        private void SaveData()
        {
            var xdoc = new XDocument(
                new XElement("Operations",
                    _operations.Select(o =>
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

            using var fs = new FileStream(_filePath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, useAsync: true);
            ms.CopyTo(fs);
            fs.Flush();
        }

        #endregion

    }
}
