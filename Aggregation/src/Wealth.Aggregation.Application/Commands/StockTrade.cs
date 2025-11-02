using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Application;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Application.Commands;

public record struct StockTrade(OperationProto Operation) : ICommand;

public class StockTradeHandler(IStockAggregationRepository repository) : ICommandHandler<StockTrade>
{
    public async Task Handle(StockTrade command, CancellationToken token)
    {
        var op = command.Operation.StockTrade;
        switch (op.Type)
        {
            case TradeOperationTypeProto.Buy:
                await repository.Buy(command.Operation.Id, op, token);
                break;
            case TradeOperationTypeProto.Sell:
                await repository.Sell(command.Operation.Id, op, token);
                break;
            case TradeOperationTypeProto.Unknown:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
