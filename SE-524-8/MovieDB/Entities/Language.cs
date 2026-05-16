namespace MovieDB.Entities
{
    public class Language
    {
        public int LanguageId { get; set; }       // LanguageID PK
        public string LanguageName { get; set; } = null!;

        // Navigation
        public ICollection<Film> Films { get; set; } = [];
    }
}
