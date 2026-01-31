namespace Lecture9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Client client = new();
            client.FirstName = "Nika";
            client.LastName = "Gvazava";
            client.PersonalNumber = "12345678901";
            client.Age = 25;
            client.Account = new Account()
            {
                Balance = 1100m
            };

            Client client2 = new();
            client2.FirstName = "Nika";
            client2.LastName = Console.ReadLine();
            client2.PersonalNumber = "12345678901";
            client2.Age = 25;
            client2.Account = new Account()
            {
                Balance = 1100m
            };


            client.Account.TransferAmount(200m, client2.Account);




        }

    }
}
