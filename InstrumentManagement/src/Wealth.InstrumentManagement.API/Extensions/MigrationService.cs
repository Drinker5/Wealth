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
        await databaseService.Migrate(cancellationToken);
        
        var seeders = scope.ServiceProvider.GetRequiredService<IEnumerable<IDbSeeder>>();
        foreach (var seeder in seeders)
            await seeder.SeedAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}