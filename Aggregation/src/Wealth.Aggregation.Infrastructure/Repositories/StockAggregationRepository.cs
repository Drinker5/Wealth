using SharpJuice.Clickhouse;
using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class StockAggregationRepository(
    IClickHouseConnectionFactory connectionFactory,
    ITableWriterBuilder tableWriterBuilder) : IStockAggregationRepository
{
    private readonly ITableWriter<StockTradeOperationProto> _tableWriter = tableWriterBuilder
        .For<StockTradeOperationProto>("stock_aggregation")
        .AddColumn("op_id", i => i.StockId.Id)
        .AddColumn("stock_id", i => i.StockId.Id)
        .Build();

    
    public Task Buy(string OpId, PortfolioId portfolioId, StockId stockId, long quantity, Money investment, CancellationToken token)
    {
        await using var connection = connectionFactory.Create();
        var command = connection.CreateCommand();
        await connection.OpenAsync(cancellationToken);
        connection.CreateColumnWriter()
    }

    public Task Buy(string OpId, StockTradeOperationProto operation, CancellationToken token)
    {
        throw new NotImplementedException();
    }

    public Task Sell(string OpId, StockTradeOperationProto operation, CancellationToken token)
    {
        throw new NotImplementedException();
    }
}