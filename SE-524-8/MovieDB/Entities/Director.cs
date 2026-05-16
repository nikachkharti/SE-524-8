namespace MovieDB.Entities
{
    public class Director
    {
        public int DirectorId { get; set; }       // DirectorID PK
        public string? FirstName { get; set; }
        public string? FamilyName { get; set; }
        public DateTime? DoB { get; set; }
        public DateTime? DoD { get; set; }
        public string? Gender { get; set; }

        // Navigation
        public ICollection<Film> Films { get; set; } = [];
    }
}
