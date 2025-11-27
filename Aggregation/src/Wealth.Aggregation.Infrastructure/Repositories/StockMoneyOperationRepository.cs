using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Repository;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class StockMoneyOperationRepository(ITableWriterBuilder tableWriterBuilder) : IStockMoneyOperationRepository
{
    private readonly ITableWriter<StockMoneyOperation> _tableWriter = tableWriterBuilder
        .For<StockMoneyOperation>("stock_money_operation")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("date", i => i.Date)
        .AddColumn("portfolio_id", i => i.PortfolioId.Value)
        .AddColumn("stock_id", i => i.StockId.Value)
        .AddColumn("amount", i => i.Amount.Amount)
        .AddColumn("currency", i => (byte)i.Amount.Currency)
        .AddColumn("type", i => (byte)i.Type)
        .Build();

    public async Task Upsert(StockMoneyOperation operation, CancellationToken token)
    {
        await _tableWriter.Insert([operation], token);
    }
}