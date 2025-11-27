using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Application.Commands;

public sealed record StockTradeOperation(
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    StockId StockId,
    long Quantity,
    Money Amount,
    TradeOperationTypeProto Type) : ICommand;

public class StockTradeOperationHandler(IStockTradeOperationRepository operationRepository) : ICommandHandler<StockTradeOperation>
{
    public async Task Handle(StockTradeOperation command, CancellationToken token)
    {
        switch (command.Type)
        {
            case TradeOperationTypeProto.Buy:
            case TradeOperationTypeProto.Sell:
                await operationRepository.Upsert(command, token);
                break;
            case TradeOperationTypeProto.Unknown:
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}