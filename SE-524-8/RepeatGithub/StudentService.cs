namespace RepeatGithub
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
    }

    public class StudentService
    {
        private readonly List<Student> _students = new();
        private int _idCounter = 1;

        public async Task SeedDataAsync()
        {
            await Task.Delay(100);
            _students.Add(new Student { Id = _idCounter++, Name = "Alice", Age = 20, Email = "alice@test.com" });
        }

        public void ListStudents()
        {
            foreach (var s in _students)
            {
                Console.WriteLine($"{s.Id} | {s.Name} | {s.Age} | {s.Email}");
            }

        }
    }
}