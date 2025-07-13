using FluentMigrator.Runner;
using Wealth.InstrumentManagement.Infrastructure.DbSeeding;
using Wealth.InstrumentManagement.Infrastructure.Migrations;

namespace Wealth.InstrumentManagement.API.Extensions;

public class MigrationService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
        var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        await databaseService.CreateDatabase("InstrumentManagement");
        migrationService.MigrateUp();
        
        var seeders = scope.ServiceProvider.GetRequiredService<IEnumerable<IDbSeeder>>();
        foreach (var seeder in seeders)
            await seeder.SeedAsync();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}