using Npgsql;
using RPAAlura.AEC.Application.Interfaces;

namespace RPAAlura.AEC.Application.Services
{
    public class DatabaseService : IDatabaseService
    {
        public void CreateDatabaseIfNotExists(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            var dbName = builder.Database;

            // Remover o nome do banco de dados para conectar à instância sem especificar um banco
            builder.Database = "postgres";

            using var connection = new NpgsqlConnection(builder.ToString());
            connection.Open();

            using (var cmd = new NpgsqlCommand($"SELECT 1 FROM pg_database WHERE datname = '{dbName}'", connection))
            {
                var result = cmd.ExecuteScalar();
                if (result == null)
                {
                    // Banco de dados não existe, criamos ele
                    using (var createDbCmd = new NpgsqlCommand($"CREATE DATABASE \"{dbName}\"", connection))
                    {
                        createDbCmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }

}
