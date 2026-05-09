using ITStepClass.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITStepClass.Data
{
    public class ApplicationDbContext : DbContext
    {
        private const string _connectionString = @"Server=DESKTOP-SCSHELD\SQLEXPRESS;Database=ClassEF;Trusted_Connection=True;TrustServerCertificate=True";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region DEPARTMENT

            modelBuilder
                .Entity<Department>()
                .HasKey(d => d.Id); //Primary Key

            modelBuilder.Entity<Department>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd(); //Identity - auto increment

            modelBuilder.Entity<Department>()
                .Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(100);

            #endregion



            #region SUBJECT

            modelBuilder
                .Entity<Subject>()
                .HasKey(d => d.Id); //Primary Key

            modelBuilder.Entity<Subject>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd(); //Identity - auto increment


            //1xM
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Subject)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion



            #region STUDENT

            modelBuilder
                .Entity<Student>()
                .HasKey(d => d.Id); //Primary Key

            modelBuilder.Entity<Student>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd(); //Identity - auto increment

            modelBuilder.Entity<Student>()
                .Property(d => d.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Student>()
                .Property(d => d.LastName)
                .IsRequired()
                .HasMaxLength(100);

            //1x1
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Profile)
                .WithOne(p => p.Student)
                .HasForeignKey<StudentProfie>(p => p.StudentId);

            #endregion


            #region STUDENT PROFILE

            modelBuilder
                .Entity<StudentProfie>()
                .HasKey(d => d.Id); //Primary Key

            modelBuilder.Entity<StudentProfie>()
                .Property(d => d.Id)
                .ValueGeneratedOnAdd(); //Identity - auto increment

            modelBuilder.Entity<StudentProfie>()
                .Property(d => d.Address)
                .IsRequired()
                .HasMaxLength(200);

            modelBuilder.Entity<StudentProfie>()
                .Property(d => d.PhoneNumber)
                .HasMaxLength(20)
                .IsRequired();

            #endregion


            #region STUDENT SUBJECT

            modelBuilder
                .Entity<StudentSubject>()
                .HasKey(ss => ss.Id);

            modelBuilder
                .Entity<StudentSubject>()
                .Property(ss => ss.Id)
                .ValueGeneratedOnAdd(); //Identity - auto increment

            modelBuilder
                .Entity<StudentSubject>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.StudentId);

            modelBuilder
                .Entity<StudentSubject>()
                .HasOne(ss => ss.Subject)
                .WithMany(s => s.StudentSubjects)
                .HasForeignKey(ss => ss.SubjectId);

            #endregion



            //Data Seeding
            modelBuilder.Entity<Department>().HasData
            (
                new Department { Id = 1, Name = "IT" },
                new Department { Id = 2, Name = "Design" },
                new Department { Id = 3, Name = "Cyber Security" }
            );

            modelBuilder.Entity<Subject>().HasData
            (
                new Subject { Id = 1, Name = "C#", DepartmentId = 1 },
                new Subject { Id = 2, Name = "JavaScript", DepartmentId = 1 },
                new Subject { Id = 3, Name = "UI/UX", DepartmentId = 2 },
                new Subject { Id = 4, Name = "Graphic Design", DepartmentId = 2 },
                new Subject { Id = 5, Name = "Network Security", DepartmentId = 3 },
                new Subject { Id = 6, Name = "Ethical Hacking", DepartmentId = 3 }
            );

            modelBuilder.Entity<Student>().HasData
            (
                new Student { Id = 1, FirstName = "John", LastName = "Doe" },
                new Student { Id = 2, FirstName = "Jane", LastName = "Smith" }
            );

            modelBuilder.Entity<StudentProfie>().HasData
            (
                new StudentProfie { Id = 1, StudentId = 1, Address = "123 Main St", PhoneNumber = "123-456-7890" },
                new StudentProfie { Id = 2, StudentId = 2, Address = "456 Elm St", PhoneNumber = "987-654-3210" }
            );

            modelBuilder.Entity<StudentSubject>().HasData
            (
                new StudentSubject { Id = 1, StudentId = 1, SubjectId = 1 },
                new StudentSubject { Id = 2, StudentId = 1, SubjectId = 2 },
                new StudentSubject { Id = 3, StudentId = 2, SubjectId = 3 },
                new StudentSubject { Id = 4, StudentId = 2, SubjectId = 4 }
            );
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentProfie> StudentProfies { get; set; }
        public DbSet<StudentSubject> StudentSubjects { get; set; }
    }
}
