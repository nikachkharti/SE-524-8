namespace MovieDB.Entities
{
    public class Certificate
    {
        public int CertificateId { get; set; }    // CertificateID PK
        public string CertificateName { get; set; } = null!;

        // Navigation
        public ICollection<Film> Films { get; set; } = [];
    }
}
