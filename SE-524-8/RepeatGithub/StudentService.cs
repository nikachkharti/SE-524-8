namespace RepeatGithub
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class StudentService
    {
        private readonly List<Student> _students = new();
        private int _idCounter = 1;

        public async Task SeedDataAsync()
        {
            await Task.Delay(50);
            _students.Add(new Student { Id = _idCounter++, Name = "Alice", Age = 20, PhoneNumber = "123456" });
        }

        public void ListStudents()
        {
            foreach (var student in _students.OrderBy(x => x.Name))
            {
                Console.WriteLine($"{student.Id} - {student.Name} - {student.Age} - {student.PhoneNumber}");
            }
        }
    }
}