using ITStepClass.Models;
using ITStepClass.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ITStepClass
{
    internal class Program
    {
        private const string _connectionString = @"Server=DESKTOP-SCSHELD\SQLEXPRESS;Database=Class;Trusted_Connection=True;TrustServerCertificate=True";

        static async Task Main(string[] args)
        {
            //var allStudents = await GetStudentsAsync();
            //var singleStudent = await GetSingleStudentAsync(2);
            //await AddNewStudentAsync(new CreateStudentDto()
            //{
            //    FirstName = "Nika",
            //    LastName = "Gogoladze"
            //});


          //  DatabaseService databaseService = new DatabaseService(_connectionString);


          //await databaseService.ExecuteAsync("AddNewStudent", new SqlParameter("FirstName", "Nika"), new SqlParameter("LastName", "Gogoladze"));


          //var rame = await databaseService.QueryAsync("GetStudents", reader => new Student()
          //  {
          //      Id = reader.GetInt32("Id"),
          //      FirstName = reader.GetString("FirstName"),
          //      LastName = reader.GetString("LastName")
          //  });
        }


        //COMMAND
        private async static Task<int> AddNewStudentAsync(CreateStudentDto model)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(@"
                INSERT INTO Students(FirstName,LastName)
                VALUES
                (@FirstName,@LastName)", connection);

            command.Parameters.AddWithValue("FirstName", model.FirstName);
            command.Parameters.AddWithValue("LastName", model.LastName);
            command.CommandType = CommandType.Text;

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync(); //გაშვება, ჩაწერა
        }

        //QUERY
        private async static Task<Student> GetSingleStudentAsync(int id)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("SELECT*FROM Students WHERE Id = @id", connection);
            command.Parameters.AddWithValue("id", id);
            command.CommandType = CommandType.Text;

            await connection.OpenAsync();

            SqlDataReader reader = await command.ExecuteReaderAsync(); //გაშვება, წაკითხვა

            while (await reader.ReadAsync())
            {
                if (reader.HasRows)
                {
                    return new Student()
                    {
                        Id = reader.GetInt32("Id"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName")
                    };
                }
            }

            return null;

        }
        private async static Task<List<Student>> GetStudentsAsync()
        {
            List<Student> students = new();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand("SELECT*FROM Students", connection);
            command.CommandType = CommandType.Text;

            await connection.OpenAsync();

            SqlDataReader reader = await command.ExecuteReaderAsync(); //გაშვება

            while (await reader.ReadAsync())
            {
                if (reader.HasRows)
                {
                    students.Add(new Student()
                    {
                        Id = reader.GetInt32("Id"),
                        FirstName = reader.GetString("FirstName"),
                        LastName = reader.GetString("LastName")
                    });
                }
            }

            return students;
        }


    }
}