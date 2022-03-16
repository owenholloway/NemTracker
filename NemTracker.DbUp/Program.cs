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

            var connectionStringBuilder = new StringBuilder();
            
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbUser = Environment.GetEnvironmentVariable("DB_USER");
            var dbPass = Environment.GetEnvironmentVariable("DB_PASS");
            var dbSchema = Environment.GetEnvironmentVariable("DB_SCHEMA");

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
            
            /*
            Console.Write(dbHost);
            Console.Write(", ");
            Console.Write(dbUser);
            Console.Write(", ");
            Console.Write(dbPass);
            Console.Write(";");
            */

            var connectionString = connectionStringBuilder.ToString();

            await using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();

        }
    }

    

}

