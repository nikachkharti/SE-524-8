namespace MovieDB.Entities
{
    public class Role
    {
        public int RoleId { get; set; }           // RoleID PK
        public string RoleName { get; set; } = null!;  // "Role" column (renamed to avoid conflict)
        public int FilmId { get; set; }
        public int ActorId { get; set; }

        // Navigation
        public Film Film { get; set; } = null!;
        public Actor Actor { get; set; } = null!;
    }
}
