using System.Data;

namespace MovieDB.Entities
{
    public class Actor
    {
        public int ActorId { get; set; }          // ActorID PK
        public string FirstName { get; set; } = null!;
        public string FamilyName { get; set; } = null!;
        public DateTime? DoB { get; set; }
        public DateTime? DoD { get; set; }
        public string? Gender { get; set; }

        // Navigation
        public ICollection<Role> Roles { get; set; } = [];
    }
}
