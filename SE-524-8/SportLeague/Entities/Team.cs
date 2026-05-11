namespace SportLeague.Entities
{
    public class Team
    {
        public int TeamId { get; set; }
        public string TeamName { get; set; } = null!;
        public string City { get; set; }
        public int Founded { get; set; }

        // Navigation
        public ICollection<Player> Players { get; set; } = new List<Player>();

        public ICollection<Game> HomeGames { get; set; } = new List<Game>();
        public ICollection<Game> AwayGames { get; set; } = new List<Game>();
    }
}
