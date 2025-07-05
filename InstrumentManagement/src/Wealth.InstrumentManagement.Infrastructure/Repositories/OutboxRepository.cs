using System.Data;
using System.Text.Json;
using Dapper;
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.InstrumentManagement.Application.Outbox;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly IDbConnection connection;

    public OutboxRepository(WealthDbContext dbContext)
    {
        connection = dbContext.CreateConnection();
    }

    public async Task Add(IDomainEvent domainEvent)
    {
        var sql = """
                  INSERT INTO "OutboxMessages" 
                  ("Id", "Type", "Data", "OccurredOn", "ProcessedOn")
                  VALUES (@Id, @Type, @Data::jsonb, @OccurredOn, NULL)
                  """;
        
        await connection.ExecuteAsync(sql, new
        {
            Id = Guid.NewGuid(),
            Type = domainEvent.GetType().Name,
            Data = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
            OccurredOn = Clock.Now,
        });
    }
}