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


            Console.WriteLine($"Hello {client.FirstName} {client.LastName} you are {client.Age} years old {client.PersonalNumber}");

        }





        static string GetFirstName()
        {
            Console.Write("FirstName: ");
            string firstName = Console.ReadLine();

            if (IsNotEmpty(firstName))
                return firstName;

            throw new ArgumentException($" FirstName can't be empty value");
        }
        static string GetLastName()
        {
            Console.Write("LastName: ");
            string lastName = Console.ReadLine();

            if (IsNotEmpty(lastName))
                return lastName;

            throw new ArgumentException($"LastName can't be empty value");
        }
        static string GetPersonalNumber()
        {
            Console.Write("Personal Number: ");
            string personalNumber = Console.ReadLine();

            if (IsNotEmpty(personalNumber) && personalNumber.Length == 11)
                return personalNumber;

            throw new ArgumentException($"Personal number can't be empty value or not equal to 11");
        }
        static byte GetAge()
        {
            Console.Write("Age: ");
            byte age = byte.Parse(Console.ReadLine());

            if (IsValidAge(age))
                return age;

            throw new ArgumentException($"Client must be adult [>=18]");
        }
        static string GetPhoneNumber()
        {
            Console.Write("Phone Number: ");
            string phoneNumber = Console.ReadLine();

            if (IsNotEmpty(phoneNumber) && phoneNumber.Length == 9)
                return phoneNumber;

            throw new ArgumentException($"Phone number can't be empty value or not equal to 9");
        }
        static decimal GetBalance()
        {
            Console.Write("Balance: ");
            decimal balance = decimal.Parse(Console.ReadLine());

            if (balance >= 0)
                return balance;

            throw new ArgumentException($"Balance can't be a negative number");
        }

        static bool IsValidAge(byte value)
        {
            return value >= 18;
        }
        static bool IsNotEmpty(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return true;
        }




    }
}
