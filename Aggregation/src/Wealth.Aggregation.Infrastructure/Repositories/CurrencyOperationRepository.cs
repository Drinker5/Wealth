using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Repository;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class CurrencyOperationRepository(ITableWriterBuilder tableWriterBuilder) : ICurrencyOperationRepository
{
    private readonly ITableWriter<CurrencyOperation> _tableWriter = tableWriterBuilder
        .For<CurrencyOperation>("currency_operation")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("date", i => i.Date)
        .AddColumn("portfolio_id", i => i.PortfolioId.Value)
        .AddColumn("amount", i => i.Amount.Amount)
        .AddColumn("currency", i => (byte)i.Amount.Currency)
        .AddColumn("type", i => (byte)i.Type)
        .Build();

    public async Task Upsert(CurrencyOperation operation, CancellationToken token)
    {
        await _tableWriter.Insert([operation], token);
    }
}