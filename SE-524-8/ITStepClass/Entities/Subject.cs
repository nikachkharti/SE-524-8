namespace ITStepClass.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //FOREIGN KEY
        public int DepartmentId { get; set; } //1
        public Department Department { get; set; } //1

        public ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
