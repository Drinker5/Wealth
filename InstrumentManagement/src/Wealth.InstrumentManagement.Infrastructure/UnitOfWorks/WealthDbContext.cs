using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

public class WealthDbContext : IDisposable
{
    private readonly IConfiguration configuration;
    private IDbConnection? connection;
    private List<IDomainEvent> trackedEvents = [];

    public WealthDbContext(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IReadOnlyList<IDomainEvent> TrackedEvents => trackedEvents.AsReadOnly();

    public IDbConnection CreateConnection()
    {
        return connection ??= new NpgsqlConnection(configuration.GetConnectionString("InstrumentManagement"));
    }

    public void AddEvents(AggregateRoot aggregate)
    {
        trackedEvents.AddRange(aggregate.DomainEvents);
    }
    
    public IDbConnection CreateMasterConnection()
        => new NpgsqlConnection(configuration.GetConnectionString("Master"));

    public void Dispose()
    {
        connection?.Dispose();
    }
}