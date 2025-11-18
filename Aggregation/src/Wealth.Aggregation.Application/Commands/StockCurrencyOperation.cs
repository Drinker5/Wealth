using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public sealed record StockCurrencyOperation(    
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    StockId StockId,
    Money Amount,
    StockCurrencyOperationType Type) : ICommand;
    
public class StockCurrencyOperationHandler(IStockCurrencyOperationRepository repository) : ICommandHandler<StockCurrencyOperation>
{
    public async Task Handle(StockCurrencyOperation command, CancellationToken token)
    {
        await repository.Upsert(command, token);
    }
}

public enum StockCurrencyOperationType : byte
{
    Dividend = 1,
    DividendTax = 2,
    BrokerFee = 3,
}