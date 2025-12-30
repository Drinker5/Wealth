using System.Runtime.CompilerServices;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public class BuyHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        InstrumentType instrumentType,
        PortfolioId portfolioId,
        [EnumeratorCancellation] CancellationToken token)
    {
        foreach (var trade in operation.Trades)
        {
            yield return instrumentType switch
            {
                InstrumentType.Bond => new BondTradeOperation
                {
                    Id = trade.TradeId,
                    Date = trade.DateTime.ToDateTimeOffset(),
                    Amount = trade.Price.ToMoney(),
                    BondId = await instrumentIdProvider.GetBondId(operation.InstrumentUid, token),
                    PortfolioId = portfolioId,
                    Quantity = trade.Quantity,
                    Type = TradeOperationType.Buy,
                },
                InstrumentType.Stock => new StockTradeOperation
                {
                    Id = trade.TradeId,
                    Date = trade.DateTime.ToDateTimeOffset(),
                    Amount = trade.Price.ToMoney(),
                    StockId = await instrumentIdProvider.GetStockId(operation.InstrumentUid, token),
                    PortfolioId = portfolioId,
                    Quantity = trade.Quantity,
                    Type = TradeOperationType.Buy,
                },
                InstrumentType.CurrencyAsset => new CurrencyTradeOperation
                {
                    Id = trade.TradeId,
                    Date = trade.DateTime.ToDateTimeOffset(),
                    Amount = trade.Price.ToMoney(),
                    CurrencyId = await instrumentIdProvider.GetCurrencyId(operation.InstrumentUid, token),
                    PortfolioId = portfolioId,
                    Quantity = trade.Quantity,
                    Type = TradeOperationType.Buy,
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}