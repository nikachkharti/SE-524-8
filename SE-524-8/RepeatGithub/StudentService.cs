namespace RepeatGithub
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

<<<<<<< HEAD
        public string Email { get; set; }
=======
        public string PhoneNumber { get; set; }
>>>>>>> feature/contact-info
    }

    public class StudentService
    {
        private readonly List<Student> _students = new();
        private int _idCounter = 1;

        public async Task SeedDataAsync()
        {
<<<<<<< HEAD
            await Task.Delay(100);
            _students.Add(new Student { Id = _idCounter++, Name = "Alice", Age = 20, Email = "alice@test.com" });
=======
            await Task.Delay(50);
            _students.Add(new Student { Id = _idCounter++, Name = "Alice", Age = 20, PhoneNumber = "123456" });
>>>>>>> feature/contact-info
        }

        public void ListStudents()
        {
<<<<<<< HEAD
            foreach (var s in _students)
            {
                Console.WriteLine($"{s.Id} | {s.Name} | {s.Age} | {s.Email}");
            }
=======
            foreach (var student in _students.OrderBy(x => x.Name))
            {
                Console.WriteLine($"{student.Id} - {student.Name} - {student.Age} - {student.PhoneNumber}");
            }
>>>>>>> feature/contact-info
        }
    }
}