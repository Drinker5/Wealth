using System.Data;
using System.Text.Json;
using Dapper;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly IDbConnection connection;

    public OutboxRepository(WealthDbContext dbContext)
    {
        connection = dbContext.CreateConnection();
    }

    public async Task Add(IntegrationEvent integrationEvent, CancellationToken cancellationToken)
    {
        var sql = """
                  INSERT INTO "OutboxMessages" 
                  ("Id", "Type", "Data", "OccurredOn", "ProcessedOn")
                  VALUES (@Id, @Type, @Data::jsonb, @OccurredOn, NULL)
                  """;
        
        await connection.ExecuteAsync(sql, new
        {
            Id = integrationEvent.Id,
            Type = integrationEvent.GetType().Name,
            Data = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType()),
            OccurredOn = Clock.Now,
        });
    }
}