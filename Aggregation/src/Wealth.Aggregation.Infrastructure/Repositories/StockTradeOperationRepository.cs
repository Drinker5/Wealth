using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Repository;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class StockTradeOperationRepository(ITableWriterBuilder tableWriterBuilder) : IStockTradeOperationRepository
{
    private readonly ITableWriter<StockTradeOperation> _tableWriter = tableWriterBuilder
        .For<StockTradeOperation>("stock_trade")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("date", i => i.Date)
        .AddColumn("portfolio_id", i => i.PortfolioId.Value)
        .AddColumn("stock_id", i => i.StockId.Value)
        .AddColumn("quantity", i => i.Quantity)
        .AddColumn("amount", i => i.Amount.Amount)
        .AddColumn("currency", i => (byte)i.Amount.Currency)
        .AddColumn("type", i => (byte)i.Type)
        .Build();

    public async Task Upsert(StockTradeOperation operation, CancellationToken token)
    {
        await _tableWriter.Insert([operation], token);
    }
}