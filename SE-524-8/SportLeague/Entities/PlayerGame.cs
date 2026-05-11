namespace SportLeague.Entities
{
    public class PlayerGame
    {
        public int PlayerId { get; set; }
        public int GameId { get; set; }

        public int GoalsScored { get; set; }
        public int MinutesPlayed { get; set; }

        public Player Player { get; set; } = null!;
        public Game Game { get; set; } = null!;
    }
}
