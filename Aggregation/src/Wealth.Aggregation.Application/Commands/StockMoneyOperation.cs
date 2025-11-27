using Wealth.Aggregation.Application.Repository;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Application.Commands;

public sealed record StockMoneyOperation(    
    string Id,
    DateTime Date,
    PortfolioId PortfolioId,
    StockId StockId,
    Money Amount,
    StockMoneyOperationType Type) : ICommand;
    
public class StockMoneyOperationHandler(IStockMoneyOperationRepository repository) : ICommandHandler<StockMoneyOperation>
{
    public async Task Handle(StockMoneyOperation command, CancellationToken token)
    {
        await repository.Upsert(command, token);
    }
}

public enum StockMoneyOperationType : byte
{
    Dividend = 1,
    DividendTax = 2,
    BrokerFee = 3,
}