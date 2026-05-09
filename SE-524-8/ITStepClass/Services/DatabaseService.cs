using Microsoft.Data.SqlClient;
using System.Data;

namespace ITStepClass.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<T> QuerySingleAsync<T>(
        string storedProcedure,
        Func<SqlDataReader, T> mapper,
        params SqlParameter[] parameters)
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);
            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();

            if (await reader.ReadAsync())
                return mapper(reader);
            return default;

        }

        public async Task<List<T>> QueryAsync<T>(
            string storedProcedure,
            Func<SqlDataReader, T> mapper,
            params SqlParameter[] parameters
        )
        {
            var result = new List<T>();

            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };

            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                if (reader.HasRows)
                    result.Add(mapper(reader));
            }

            return result;
        }

        public async Task<int> ExecuteAsync(
            string storedProcedure,
            params SqlParameter[] parameters
        )
        {
            await using var connection = new SqlConnection(_connectionString);
            await using var command = new SqlCommand(storedProcedure, connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            if (parameters.Length > 0)
                command.Parameters.AddRange(parameters);
            await connection.OpenAsync();
          return await command.ExecuteNonQueryAsync();

           
        }
    }

}
