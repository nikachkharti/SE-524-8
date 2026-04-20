namespace RepeatGithub
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Student Management System ===");

            var service = new StudentService();

            await service.SeedDataAsync();

            service.ListStudents();
        }
    }
}
