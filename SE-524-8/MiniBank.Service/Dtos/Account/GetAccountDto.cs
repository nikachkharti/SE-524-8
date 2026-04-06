namespace MiniBank.Service.Dtos.Account
{
    public class GetAccountDto
    {
        public int Id { get; set; }
        public string Iban { get; set; }
        public string Currency { get; set; }
        public decimal Balance { get; set; }

        public override string ToString() => $"{Iban} | {Balance} {Currency}";
    }
}
