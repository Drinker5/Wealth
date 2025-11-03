using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class StockAggregationRepository(
    IClickHouseConnectionFactory connectionFactory,
    ITableWriterBuilder tableWriterBuilder) : IStockAggregationRepository
{
    private readonly ITableWriter<StockBuy> _tableWriter = tableWriterBuilder
        .For<StockBuy>("stock_aggregation")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("stock_id", i => i.Operation.StockId.Id)
        .AddColumn("portfolio_id", i => i.Operation.PortfolioId.Id)
        .AddColumn("amount", i => i.Operation.Amount.Amount)
        .Build();


    public Task Buy(StockTrade operation, CancellationToken token)
    {
        await using var connection = connectionFactory.Create();
        var command = connection.CreateCommand();
        await connection.OpenAsync(cancellationToken);
        connection.CreateColumnWriter()
    }

    public Task Sell(StockTrade operation, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}