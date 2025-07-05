using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

public class WealthDbContext : IDisposable
{
    private readonly IConfiguration configuration;
    private IDbConnection? connection;

    public WealthDbContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IDbConnection CreateConnection()
    {
        return connection ??= new NpgsqlConnection(configuration.GetConnectionString("InstrumentManagement"));
    }

    public IDbConnection CreateMasterConnection()
        => new NpgsqlConnection(configuration.GetConnectionString("Master"));

    public void Dispose()
    {
        connection?.Dispose();
    }
}