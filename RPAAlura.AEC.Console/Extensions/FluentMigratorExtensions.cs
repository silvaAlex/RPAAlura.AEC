using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using RPAAlura.AEC.Infra.Migrations;

namespace RPAAlura.AEC.Console.Extensions
{
    public static class FluentMigratorExtensions
    {
        public static IServiceCollection AddFluentMigratorForPostgres(this IServiceCollection services, string connectionString)
        {
            services
               .AddFluentMigratorCore()
               .ConfigureRunner(rb => rb
                   .AddPostgres()
                   .WithGlobalConnectionString(connectionString)
                   .ScanIn(typeof(CreateCursoTable).Assembly).For.Migrations())
               .AddLogging(lb => lb.AddFluentMigratorConsole());

            return services;
        }
    }
}
