namespace Lecture13
{
    public class Team
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int FoundationYear { get; set; }
        public string Stadium { get; set; }
        public int Wins { get; set; }

        public static Team Parse(string input)
        {
            string[] splitedResult = input.Split('|');

            if (splitedResult.Length != 7)
                throw new FormatException("File format is incorret");

            Team team = new Team();

            team.Id = int.Parse(splitedResult[0]);
            team.Title = splitedResult[1];
            team.Country = splitedResult[2];
            team.City = splitedResult[3];
            team.FoundationYear = int.Parse(splitedResult[4]);
            team.Stadium = splitedResult[5];
            team.Wins = int.Parse(splitedResult[6]);

            return team;
        }

        public int GetWinsCount()
        {
            return Wins;
        }


        public override string ToString()
        {
            return $"{Id} | {Title} | {City} | {Country} | {FoundationYear} | {Stadium} | {Wins}";
        }
    }


    public abstract class Bird
    {
        public string Name { get; set; }
    }

    public interface IFlyer
    {
        public string TestPropery { get; set; }

        public void Fly();
    }

    public interface IFlyer2
    {
        public int TestProperty2 { get; set; }
        public void Fly();
    }

    public interface IFlyer3 : IFlyer2, IFlyer
    {

    }

    public class Eagle : Bird, IFlyer, IFlyer2
    {
        public string TestPropery { get; set; }
        public int TestProperty2 { get; set; }

        void IFlyer.Fly() // ცხადი სახით იმპლემენტაცია
        {
            throw new NotImplementedException();
        }

        void IFlyer2.Fly() // ცხადი სახით იმპლემენტაცია
        {
            throw new NotImplementedException();
        }
    }


    public class Penguin : Bird
    {
    }



    internal class Program
    {
        static void Main(string[] args)
        {

            //Bird a = new Eagle();
            //IFlyer a = new Penguin();


        }

    }
}
