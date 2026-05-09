namespace ITStepClass.Entities
{
    public class StudentProfie
    {
        public int Id { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        //Foreign key to Student
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
