namespace MovieDB.Entities
{
    public class Film
    {
        public int FilmId { get; set; }     // GenreID PK
        public string Title { get; set; } = null!;
        public DateTime? ReleaseDate { get; set; }
        public int? DirectorId { get; set; }
        public int? StudioId { get; set; }
        public string? Review { get; set; }
        public int? CountryId { get; set; }
        public int? LanguageId { get; set; }
        public int? GenreId { get; set; }
        public short? RunTimeMinutes { get; set; }
        public int? CertificateId { get; set; }
        public long? BudgetDollars { get; set; }
        public long? BoxOfficeDollars { get; set; }
        public byte? OscarNominations { get; set; }
        public byte? OscarWins { get; set; }

        // Navigation properties
        public Director Director { get; set; } = null!;
        public Studio Studio { get; set; } = null!;
        public Country Country { get; set; } = null!;
        public Language Language { get; set; } = null!;
        public Genre Genre { get; set; } = null!;
        public Certificate Certificate { get; set; } = null!;
        public ICollection<Role> Roles { get; set; } = [];
    }
}
