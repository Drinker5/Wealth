using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement;

namespace Wealth.Aggregation.Application.Commands;

public sealed record CurrencyTradeOperation(
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    CurrencyId CurrencyId,
    long Quantity,
    Money Amount,
    TradeOperationTypeProto Type) : ICommand;

public class CurrencyTradeOperationHandler(ICurrencyTradeOperationRepository operationRepository) : ICommandHandler<CurrencyTradeOperation>
{
    public async Task Handle(CurrencyTradeOperation command, CancellationToken token)
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