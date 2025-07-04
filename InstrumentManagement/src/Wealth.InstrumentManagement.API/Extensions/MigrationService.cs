using FluentMigrator.Runner;
using Wealth.InstrumentManagement.Infrastructure.DbSeeding;
using Wealth.InstrumentManagement.Infrastructure.Migrations;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.API.Extensions;

public class MigrationService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();
        var databaseService = scope.ServiceProvider.GetRequiredService<Database>();
        var migrationService = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        await databaseService.CreateDatabase("instrumentmanagement");
        migrationService.MigrateUp();
        
        var dbContext = serviceProvider.GetRequiredService<WealthDbContext>();
        var seeders = serviceProvider.GetRequiredService<IEnumerable<IDbSeeder>>();
        foreach (var seeder in seeders)
            await seeder.SeedAsync(dbContext);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}