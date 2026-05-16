namespace MovieDB.Entities
{
    public class Studio
    {
        public int StudioId { get; set; }         // StudioID PK
        public string? StudioName { get; set; }

        // Navigation
        public ICollection<Film> Films { get; set; } = [];
    }
}
