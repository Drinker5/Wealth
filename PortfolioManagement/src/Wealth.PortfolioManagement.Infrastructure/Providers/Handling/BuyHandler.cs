using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

public class BuyHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        InstrumentType instrumentType,
        PortfolioId portfolioId)
    {
        foreach (var trade in operation.Trades)
        {
            yield return instrumentType switch
            {
                InstrumentType.Bond => new BondTradeOperation
                {
                    Id = trade.TradeId,
                    Date = trade.DateTime.ToDateTimeOffset(),
                    Amount = new Money(operation.Currency, trade.Price),
                    BondId = await instrumentIdProvider.GetBondIdByFigi(operation.Figi),
                    PortfolioId = portfolioId,
                    Quantity = trade.Quantity,
                    Type = TradeOperationType.Buy,
                },
                InstrumentType.Share => new StockTradeOperation
                {
                    Id = trade.TradeId,
                    Date = trade.DateTime.ToDateTimeOffset(),
                    Amount = new Money(operation.Currency, trade.Price),
                    StockId = await instrumentIdProvider.GetStockIdByFigi(operation.Figi),
                    PortfolioId = portfolioId,
                    Quantity = trade.Quantity,
                    Type = TradeOperationType.Buy,
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}