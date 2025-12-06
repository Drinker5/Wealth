using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public class SellHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        Tinkoff.InvestApi.V1.InstrumentType instrumentType,
        PortfolioId portfolioId)
    {
        foreach (var trade in operation.Trades)
        {
            yield return instrumentType switch
            {
                Tinkoff.InvestApi.V1.InstrumentType.Bond => new BondTradeOperation
                {
                    Id = trade.TradeId,
                    Date = trade.DateTime.ToDateTimeOffset(),
                    Amount = trade.Price.ToMoney(),
                    BondId = await instrumentIdProvider.GetBondIdByFigi(operation.Figi),
                    PortfolioId = portfolioId,
                    Quantity = -trade.Quantity,
                    Type = TradeOperationType.Sell,
                },
                Tinkoff.InvestApi.V1.InstrumentType.Share => new StockTradeOperation
                {
                    Id = trade.TradeId,
                    Date = trade.DateTime.ToDateTimeOffset(),
                    Amount = trade.Price.ToMoney(),
                    StockId = await instrumentIdProvider.GetStockIdByFigi(operation.Figi),
                    PortfolioId = portfolioId,
                    Quantity = -trade.Quantity,
                    Type = TradeOperationType.Sell,
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}