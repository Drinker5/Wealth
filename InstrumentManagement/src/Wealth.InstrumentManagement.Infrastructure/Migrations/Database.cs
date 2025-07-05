using Dapper;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Migrations;

public class Database
{
    private readonly WealthDbContext context;
    public Database(WealthDbContext context)
    {
        this.context = context;
    }

    public async Task CreateDatabase(string dbName)
    {
        var query = "SELECT datname FROM pg_database WHERE datname = @name";
        var parameters = new DynamicParameters();
        parameters.Add("name", dbName);
        using var connection = context.CreateMasterConnection();
        var records = connection.Query(query, parameters);
        if (!records.Any())
            await connection.ExecuteAsync($"CREATE DATABASE \"{dbName}\"");
    }
}