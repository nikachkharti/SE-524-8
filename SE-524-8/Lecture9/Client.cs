namespace Lecture9
{
    public class Client
    {
        //ენკაფსულაცია - encapsulation
        //FULL property - თვისება
        private string personalNumber;
        public string PersonalNumber
        {
            get { return this.personalNumber; }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length == 11)
                    this.personalNumber = value;
            }
        }


        //ენკაფსულაცია - encapsulation
        //FULL property - თვისება
        private int age;
        public int Age
        {
            get { return this.age; }
            set
            {
                if (value > 0)
                    this.age = value;
            }
        }


        //AUTO property - თვისება
        public string FirstName { get; set; }

        //AUTO property - თვისება
        public string LastName { get; set; }










        //public string phoneNumber;
        //public decimal balance;
        //public DateTime registerDate;






        //public void TransferAmount(decimal amount, Client receiver)
        //{
        //    if (this.balance < amount)
        //        throw new InvalidOperationException("Not enough balance to perform the transfer.");

        //    this.balance -= amount;
        //    receiver.balance += amount;
        //}
    }
}
