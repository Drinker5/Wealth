using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

public class WealthDbContext(IConfiguration configuration) : IDisposable
{
    private IDbConnection? connection;
    private readonly List<IDomainEvent> trackedEvents = [];

    public IReadOnlyList<IDomainEvent> TrackedEvents => trackedEvents.AsReadOnly();

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
}