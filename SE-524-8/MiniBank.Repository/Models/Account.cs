namespace MiniBank.Repository.Models
{
    public class Account
    {
        //სავალდებულო
        //დადებითი
        public int Id { get; set; }

        //სავალდებულო
        //ზუტად ზომაში 22
        public string Iban { get; set; }

        //სავალდებულო
        //ზუტად ზომაში 3
        //თუ შემოიყვანეს gel --> GEL
        public string Currency { get; set; }
        public decimal Balance { get; set; }

        //სავალდებულო
        public int CustomerId { get; set; }
        public string Destination { get; set; }
    }
}
