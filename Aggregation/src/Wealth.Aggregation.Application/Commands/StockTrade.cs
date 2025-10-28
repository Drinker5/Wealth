using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Application.Commands;

public record struct StockTrade(StockTradeOperationProto Operation) : ICommand;

public class StockTradeHandler(IStockAggregationRepository repository) : ICommandHandler<StockTrade>
{
    public async Task Handle(StockTrade command, CancellationToken cancellationToken)
    {
        await repository.ProcessTrade(command.Operation, cancellationToken);
    }
}
