using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;
using RPAAlura.AEC.Application.Helper;
using RPAAlura.AEC.Application.Interfaces;
using RPAAlura.AEC.Application.Services;
using RPAAlura.AEC.Console.Extensions;
using RPAAlura.AEC.Console.Handler;
using RPAAlura.AEC.Console.Migrations;
using RPAAlura.AEC.Domain.Entities;
using RPAAlura.AEC.Infra.Database;
using RPAAlura.AEC.Infra.Repository;
using RPAAlura.AEC.Application.Factory;
using RPAAlura.AEC.Application.Exceptions;
using System.Web;
using RPAAlura.AEC.Infra.Services;


class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        var configuration = BuildConfiguration();
        List<string>? urls = null;

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        string? userInput = Environment.GetEnvironmentVariable("CURSO");

        UrlTaskHandler? urlTaskHandler;
        CursoTaskHandler? cursoTaskHandler;

        if (connectionString != null)
        {
            ConfigureServices(services, connectionString);
            var serviceProvider = services.BuildServiceProvider();

            CreateDatabase(connectionString, serviceProvider);

            urlTaskHandler = new UrlTaskHandler(serviceProvider);
            cursoTaskHandler = new CursoTaskHandler(serviceProvider);
        }
        else
        {
            throw new Exception($"Erro: A connectionstring não foi configurado");
        }

        if (urlTaskHandler != null)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                throw new RpaTaskException($"User input cannot be null or empty. {nameof(userInput)}");
            }

            string decodedString = HttpUtility.HtmlDecode(userInput);

            urls = urlTaskHandler.GetUrls(decodedString);
        }

        if (cursoTaskHandler != null)
        {
            if (urls != null && urls.Count > 0)
            {
                 cursoTaskHandler.ExecuteCursoTasks(urls);
            }
        }
    }

    private static void CreateDatabase(string connectionString, ServiceProvider serviceProvider)
    {
        var databaseService = serviceProvider.GetService<IDatabaseService>();
        databaseService?.CreateDatabaseIfNotExists(connectionString);

        using (var scope = serviceProvider.CreateScope())
        {
            scope.ServiceProvider.RunMigrations();
        }
    }

    private static void ConfigureServices(IServiceCollection services, string connectionString)
    {

        services.AddSingleton(provider =>
        {
            IWebDriver webDriver = WebDriverFactory.CreateWebDriver();
            return new WebDriverHelper(webDriver);
        });

        services.AddSingleton(new DbConnectionFactory(connectionString).CreateConnection());
        services.AddTransient<IDatabaseService, DatabaseService>();
        services.AddTransient<IRpaRepository<Curso>, NpgsqlRpaCursoRepository>();
        services.AddTransient<IRpaTaskService<UrlSearchTaskEntity>, RpaTaskGetUrlsService>();
        services.AddTransient<IRpaTaskService<CursoTaskEntity>, RpaTaskGetCursoService>();
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
