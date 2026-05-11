namespace SportLeague.Entities
{
    public class PlayerProfile
    {
        public int ProfileId { get; set; }

        public int PlayerId { get; set; }

        public decimal Height_cm { get; set; }
        public decimal Weight_kg { get; set; }

        public string Nationality { get; set; }

        public Player Player { get; set; } = null!;
    }
}
