using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Repository;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class StockCurrencyOperationRepository(ITableWriterBuilder tableWriterBuilder) : IStockCurrencyOperationRepository
{
    private readonly ITableWriter<StockCurrencyOperation> _tableWriter = tableWriterBuilder
        .For<StockCurrencyOperation>("stock_currency_operation")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("date", i => i.Date)
        .AddColumn("portfolio_id", i => i.PortfolioId.Value)
        .AddColumn("stock_id", i => i.StockId.Value)
        .AddColumn("amount", i => i.Amount.Amount)
        .AddColumn("currency", i => (byte)i.Amount.CurrencyId.Value)
        .AddColumn("type", i => (byte)i.Type)
        .Build();

    public async Task Upsert(StockCurrencyOperation operation, CancellationToken token)
    {
        await _tableWriter.Insert([operation], token);
    }
}