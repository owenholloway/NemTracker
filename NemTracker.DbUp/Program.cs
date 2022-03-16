using System;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace NemTracker.DbUp
{
    
    internal static class Program
    {
        private static async Task Main(string[] args)
        {

            var dbHost 
                = Environment.GetEnvironmentVariable("DB_HOST");
            var dbUser 
                = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass 
                = Environment.GetEnvironmentVariable("DB_PASS");
            var dbSchema 
                = Environment.GetEnvironmentVariable("DB_SCHEMA");

            var connectionStringBuilder = new StringBuilder();
            connectionStringBuilder.Append("Host=");
            connectionStringBuilder.Append(dbHost);
            connectionStringBuilder.Append(';');
            connectionStringBuilder.Append("Username=");
            connectionStringBuilder.Append(dbUser);
            connectionStringBuilder.Append(';');
            connectionStringBuilder.Append("Password=");
            connectionStringBuilder.Append(dbPass);
            connectionStringBuilder.Append(';');
            connectionStringBuilder.Append("Database=");
            connectionStringBuilder.Append(dbSchema);

            var connectionString = connectionStringBuilder.ToString();

            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();



            if (CheckInit(connection))
            {
                Console.WriteLine("Init DB");
                connection.ExecuteFile("./Transforms/Init.sql");
            }
            
            
                
            foreach (var file in  System.IO.Directory.GetFiles("./Transforms/Strcture/"))
            {
                Console.WriteLine(file);
            }
            
        }

        private static int ExecuteFile(this NpgsqlConnection connection, string filename)
        {
            string fileSql = System.IO.File.ReadAllText(filename, Encoding.UTF8);
            var command = new NpgsqlCommand(fileSql, connection);
            return command.ExecuteNonQuery();
        }

        private static bool CheckInit(NpgsqlConnection connection)
        {
            string fileSql = System.IO.File.ReadAllText("./Transforms/CheckInit.sql", Encoding.UTF8);
            var command = new NpgsqlCommand(fileSql, connection);
            var reader = command.ExecuteReader();

            bool needsInit = false;

            var result = reader;
            
            while (reader.Read())
            {
                if (!reader[0].ToString()!.Contains("True"))
                {
                    needsInit = true;
                }
            }

            while (!reader.IsClosed)
            {
                Console.WriteLine("Waiting for Close");
                reader.Close();
            }

            return needsInit;
        }

    }

}

