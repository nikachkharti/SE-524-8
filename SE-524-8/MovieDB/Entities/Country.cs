namespace MovieDB.Entities
{
    public class Country
    {
        public int CountryId { get; set; }        // CountryID PK
        public string CountryName { get; set; } = null!;

        // Navigation
        public ICollection<Film> Films { get; set; } = [];
    }
}
