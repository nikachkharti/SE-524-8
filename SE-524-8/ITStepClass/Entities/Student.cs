namespace ITStepClass.Entities
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public StudentProfie Profile { get; set; }

        public ICollection<StudentSubject> StudentSubjects { get; set; }
    }
}
