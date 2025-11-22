using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Repository;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class StockTradeRepository(ITableWriterBuilder tableWriterBuilder) : IStockTradeRepository
{
    private readonly ITableWriter<StockTrade> _tableWriter = tableWriterBuilder
        .For<StockTrade>("stock_trade")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("date", i => i.Date)
        .AddColumn("portfolio_id", i => i.PortfolioId.Value)
        .AddColumn("stock_id", i => i.StockId.Value)
        .AddColumn("quantity", i => i.Quantity)
        .AddColumn("amount", i => i.Amount.Amount)
        .AddColumn("currency", i => (byte)i.Amount.Currency)
        .AddColumn("type", i => (byte)i.Type)
        .Build();


    public async Task Upsert(StockTrade operation, CancellationToken token)
    {
        await _tableWriter.Insert([operation], token);
    }
}