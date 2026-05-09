namespace ITStepClass.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Subject> Subject { get; set; } //M
    }
}
