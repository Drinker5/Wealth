using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Infrastructure.Mediation;
using Wealth.BuildingBlocks.Infrastructure.Repositories;

namespace Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

public class WealthDbContext(IConfiguration configuration) : 
    IDisposable,
    IDomainEventsResolver, 
    IConnectionFactory,
    IEventTracker
{
    private NpgsqlConnection? connection;
    private readonly List<DomainEvent> trackedEvents = [];

    public IDbConnection CreateConnection()
    {
        return connection ??= new NpgsqlConnection(configuration.GetConnectionString("InstrumentManagement"));
    }

    public void AddEvents(AggregateRoot aggregate)
    {
        if (aggregate.DomainEvents != null)
            trackedEvents.AddRange(aggregate.DomainEvents);
    }

    public IDbConnection CreateMasterConnection()
        => new NpgsqlConnection(configuration.GetConnectionString("Master"));

    public void Dispose()
    {
        connection?.Dispose();
    }

    public IReadOnlyCollection<DomainEvent> Resolve()
    {
        var events = trackedEvents.ToList();
        trackedEvents.Clear();
        return events;
    }
}