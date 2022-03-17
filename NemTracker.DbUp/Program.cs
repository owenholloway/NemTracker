using System;
using System.Data;
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



            if (connection.CheckInit())
            {
                Console.WriteLine("Init DB");
            }
            
            foreach (var file in  System.IO.Directory.GetFiles("./Transforms/Structure/"))
            {
                var fileName = file.Replace("./Transforms/Structure/", "");
                if (!connection.HasBeenRun(fileName))
                {
                    Console.WriteLine("Running: " + fileName);
                    connection.ExecuteFile(fileName);
                }
                else
                {
                    Console.WriteLine("Exists: " + fileName);
                }
            }
            
        }

        private static int ExecuteFile(this NpgsqlConnection connection, string filename, bool recordRun = true)
        {
            var filePath = "./Transforms/Structure/" + filename;
            string fileSql = System.IO.File.ReadAllText(filePath, Encoding.UTF8);
            var command = new NpgsqlCommand(fileSql, connection);
            var result = command.ExecuteNonQuery();

            if (!recordRun) return 0;
            
            var recordCommandString = $"INSERT INTO schema_versions VALUES ('{filename}',now())";
            var recordCommand = new NpgsqlCommand(recordCommandString, connection);
            var recordResult = recordCommand.ExecuteNonQuery();

            return 0;
        }

        private static bool CheckInit(this NpgsqlConnection connection)
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

            if (needsInit)
            {
                var fileInit = "./Transforms/Init.sql";
                string initSql = System.IO.File.ReadAllText(fileInit, Encoding.UTF8);
                var commandSql = new NpgsqlCommand(initSql, connection);
                var resultInit = commandSql.ExecuteNonQuery();
            }

            return needsInit;
        }
        
        private static bool HasBeenRun(this NpgsqlConnection connection, string fileName)
        {
            var commandString = $"SELECT EXISTS ( SELECT FROM schema_versions WHERE name = '{fileName}' )";

            var command = new NpgsqlCommand(commandString, connection);
            var reader = command.ExecuteReader();

            bool exist = false;

            while (reader.Read())
            {
                if (reader[0].ToString()!.Contains("True"))
                {
                    exist = true;
                }
            }

            while (!reader.IsClosed)
            {
                reader.Close();
            }

            return exist;
        }

    }

}

