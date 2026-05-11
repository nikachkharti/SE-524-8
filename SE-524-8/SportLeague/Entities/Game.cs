namespace SportLeague.Entities
{
    public class Game
    {
        public int GameId { get; set; }

        public DateTime GameDate { get; set; }

        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        public string Score { get; set; }

        public Team HomeTeam { get; set; } = null!;
        public Team AwayTeam { get; set; } = null!;

        public ICollection<PlayerGame> PlayerGames { get; set; } = new List<PlayerGame>();
    }
}
