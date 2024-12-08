using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RPAAlura.AEC.Application.Interfaces;
using RPAAlura.AEC.Application.Services;
using RPAAlura.AEC.Console.Extensions;
using RPAAlura.AEC.Console.Migrations;
using RPAAlura.AEC.Domain.Entities;
using RPAAlura.AEC.Infra.Database;
using RPAAlura.AEC.Infra.Repository;


class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        var configuration = BuildConfiguration();

        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (connectionString != null)
        {
            ConfigureServices(services, connectionString);

            var serviceProvider = services.BuildServiceProvider();

            var databaseService = serviceProvider.GetService<IDatabaseService>();
            databaseService?.CreateDatabaseIfNotExists(connectionString);

            using (var scope = serviceProvider.CreateScope())
            {
                scope.ServiceProvider.RunMigrations();
            }
        }
        else
        {
            throw new Exception($"Erro: A connectionstring não foi configurado");
        }
    }

    private static void ConfigureServices(IServiceCollection services, string connectionString)
    {
        services.AddSingleton(new DbConnectionFactory(connectionString).CreateConnection());
        services.AddTransient<IDatabaseService, DatabaseService>();
        services.AddTransient<IRpaRepository<Curso>, NpgsqlRpaCursoRepository>();
        services.AddFluentMigratorForPostgres(connectionString);
    }

    private static IConfiguration BuildConfiguration()
    {
        return new ConfigurationBuilder()
           .SetBasePath (Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .AddEnvironmentVariables()
           .AddCommandLine(Environment.GetCommandLineArgs())
           .Build();
    }
}
