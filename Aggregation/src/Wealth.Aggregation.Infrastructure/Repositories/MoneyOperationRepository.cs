using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Repository;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class MoneyOperationRepository(ITableWriterBuilder tableWriterBuilder) : IMoneyOperationRepository
{
    private readonly ITableWriter<MoneyOperation> _tableWriter = tableWriterBuilder
        .For<MoneyOperation>("money_operation")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("date", i => i.Date)
        .AddColumn("portfolio_id", i => i.PortfolioId.Value)
        .AddColumn("amount", i => i.Amount.Amount)
        .AddColumn("currency", i => (byte)i.Amount.Currency)
        .AddColumn("type", i => (byte)i.Type)
        .Build();

    public async Task Upsert(MoneyOperation operation, CancellationToken token)
    {
        await _tableWriter.Insert([operation], token);
    }
}