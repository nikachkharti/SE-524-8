using ITStepClass.Data;
using ITStepClass.Entities;

namespace ITStepClass
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                using var db = new ApplicationDbContext();
                await db.Database.EnsureCreatedAsync();

            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }

        private static async Task AddNewDepartment(ApplicationDbContext db)
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();

            await db.Departments.AddAsync(new Department { Name = name });
            await db.SaveChangesAsync(); // !!!!!!!
        }
    }
}