namespace Lecture9
{
    public class Client
    {
        public string firstName;
        public string lastName;
        public string personalNumber;
        public byte age;
        public string phoneNumber;
        public decimal balance;
        public DateTime registerDate;

        public string SayHello()
        {
            return $"{firstName} | {lastName} | {personalNumber} | {age} | {phoneNumber} | {balance} | {registerDate}";
        }

    }
}
