using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

public class WealthDbContext
{
    private readonly IConfiguration configuration;

    public WealthDbContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IDbConnection CreateConnection()
        => new NpgsqlConnection(configuration.GetConnectionString("InstrumentManagement"));

    public IDbConnection CreateMasterConnection()
        => new NpgsqlConnection(configuration.GetConnectionString("Master"));
}