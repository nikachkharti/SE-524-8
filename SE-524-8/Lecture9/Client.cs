namespace Lecture9
{
    public class Client
    {
        public DateTime RegistrationDate { get; set; }
        private string personalNumber;
        private int age;
        private string firstName;
        private string lastName;
        private string phoneNumber;

        public string PersonalNumber
        {
            get { return this.personalNumber; }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Length == 11)
                    this.personalNumber = value;
            }
        }
        public int Age
        {
            get { return this.age; }
            set
            {
                if (value > 0)
                    this.age = value;
            }
        }
        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    firstName = value;
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                    lastName = value;
            }
        }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                if (value.Length == 9)
                {
                    phoneNumber = value;
                }
            }
        }

        public Account Account { get; set; }

    }
}
