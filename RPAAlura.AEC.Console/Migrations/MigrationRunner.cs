using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace RPAAlura.AEC.Console.Migrations
{
    public static class MigrationRunner
    {
		public static void RunMigrations(this IServiceProvider serviceProvider)
		{
			try
			{
                var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                runner.MigrateUp();
            }
			catch (Exception ex)
			{
                throw new Exception($"ocorreu um erro ao criar a migração. Erro: {ex}"); ;
			}
		}
    }
}
