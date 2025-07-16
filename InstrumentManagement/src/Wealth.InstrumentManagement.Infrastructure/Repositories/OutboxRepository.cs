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
                  ("Id", "Type", "Data", "OccurredOn", "ProcessedOn", "Key")
                  VALUES (@Id, @Type, @Data::jsonb, @OccurredOn, NULL, @Key)
                  """;

        var outboxMessage = integrationEvent.ToOutboxMessage(Serialize);

        await connection.ExecuteAsync(sql, new 
        {
            Id = outboxMessage.Id,
            Type = outboxMessage.Type,
            Data = outboxMessage.Data,
            OccurredOn = outboxMessage.OccurredOn,
            Key = outboxMessage.Key
        });

        string Serialize(IntegrationEvent arg)
        {
            return JsonSerializer.Serialize(arg, arg.GetType());
        }
    }
}