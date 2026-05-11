namespace SportLeague.Entities
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Position { get; set; }

        public int TeamId { get; set; }

        // Navigation
        public Team Team { get; set; } = null!;
        public PlayerProfile PlayerProfile { get; set; } = null!;

        public ICollection<PlayerGame> PlayerGames { get; set; } = new List<PlayerGame>();
    }
}
