using Dapper;
using FluentMigrator.Runner;
using Wealth.BuildingBlocks.Infrastructure.Repositories;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

public class Database(IConnectionFactory connectionFactory, IMigrationRunner migrationRunner)
{
    public async Task Migrate(CancellationToken token)
    {
        await CreateDatabase("InstrumentManagement", token);
        migrationRunner.MigrateUp();
    }

    private async Task CreateDatabase(string dbName, CancellationToken token)
    {
        var parameters = new CommandDefinition(
            "SELECT datname FROM pg_database WHERE datname = @name",
            new
            {
                name = dbName
            },
            cancellationToken: token);
        using var connection = connectionFactory.CreateMasterConnection();
        var records = await connection.QueryAsync(parameters);
        if (!records.Any())
            await connection.ExecuteAsync($"CREATE DATABASE \"{dbName}\"");
    }
}