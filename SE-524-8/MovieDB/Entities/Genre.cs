namespace MovieDB.Entities
{
    public class Genre
    {
        public int GenreId { get; set; }          // GenreID PK
        public string? GenreName { get; set; }

        // Navigation
        public ICollection<Film> Films { get; set; } = [];
    }
}
