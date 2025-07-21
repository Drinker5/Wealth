using System.Data;
using Dapper;
using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class OutboxRepository : IOutboxRepository
{
    private readonly IDbConnection connection;

    public OutboxRepository(WealthDbContext dbContext)
    {
        connection = dbContext.CreateConnection();
    }

    public async Task Add(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        var sql = """
                  INSERT INTO "OutboxMessages" 
                  ("Id", "Type", "Data", "OccurredOn", "ProcessedOn", "Key")
                  VALUES (@Id, @Type, @Data, @OccurredOn, NULL, @Key)
                  """;

        await connection.ExecuteAsync(sql, new 
        {
            Id = outboxMessage.Id,
            Type = outboxMessage.Type,
            Data = outboxMessage.Data,
            OccurredOn = outboxMessage.OccurredOn,
            Key = outboxMessage.Key
        });
    }
}