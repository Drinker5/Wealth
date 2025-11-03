using System.Runtime.InteropServices;
using Wealth.Aggregation.Domain;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Application.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct StockTrade(
    string Id,
    PortfolioId PortfolioId,
    StockId StockId,
    long Quantity,
    Money Amount,
    TradeOperationTypeProto Type) : ICommand;

public class StockTradeHandler(IStockAggregationRepository repository) : ICommandHandler<StockTrade>
{
    public async Task Handle(StockTrade command, CancellationToken token)
    {
        switch (command.Type)
        {
            case TradeOperationTypeProto.Buy:
                await repository.Buy(command, token);
                break;
            case TradeOperationTypeProto.Sell:
                await repository.Sell(command, token);
                break;
            case TradeOperationTypeProto.Unknown:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}