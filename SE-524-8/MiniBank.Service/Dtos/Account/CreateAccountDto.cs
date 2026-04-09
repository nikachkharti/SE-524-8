namespace MiniBank.Service.Dtos.Account
{
    public class CreateAccountDto
    {
        public string Iban { get; set; }
        public string Currency { get; set; }
        public string Destination { get; set; }
    }
}
