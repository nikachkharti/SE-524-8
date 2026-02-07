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


    internal class Program
    {
        static void Main(string[] args)
        {

            //Team baracaObj = Team.Parse("1 | FC Barcelona | Spain | Barcelona | 1899 | Camp Nou | 27");
            //baracaObj.GetWinsCount();

            //Team realObj = Team.Parse("2 | Real Madrid | Spain | Madrid | 1902 | Santiago Bernabeu | 35");

            //realObj.GetWinsCount();

        }

    }
}
