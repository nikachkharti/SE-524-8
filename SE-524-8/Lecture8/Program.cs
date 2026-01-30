class Client // გამოვაცხადე კლიენტის კლასი
{
    public string firstName;
    public string lastName;
    public byte age;

    public Client(string firstName, string lastName, byte age) // კონსტრუქტორი
    {
        this.firstName = firstName;
        this.lastName = lastName;
        this.age = age;
    }

    public Client()
    {
    }

}

internal class Program
{
    static void Main(string[] args)
    {
        Client tornikesObj = new Client();
        Client giorgisObj = new Client("Giorgi", "Ninikashvili", 28); // კონსტრუქტორი


        #region ობიექტის შექმნის მაგალითი
        //Client nikasObj = new Client(); // შევქმენი კლიენტის ობიექტი
        //nikasObj.FirstName = "Nika";
        //nikasObj.LastName = "Chkharithsvli";
        //nikasObj.Age = 21;

        //Client shotasObj = new Client()
        //{
        //    FirstName = "Shota",
        //    LastName = "Akhvlediani",
        //    Age = 25
        //};

        //Client[] clients = [
        //    nikasObj,
        //    shotasObj,
        //    new Client()
        //    {
        //        FirstName = "Beqa",
        //        LastName = "Beberashvili",
        //        Age = 30
        //    }
        //]; 
        #endregion


    }

}
