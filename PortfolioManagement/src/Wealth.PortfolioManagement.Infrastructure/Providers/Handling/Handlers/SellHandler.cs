using System.Runtime.CompilerServices;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public class SellHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        Tinkoff.InvestApi.V1.InstrumentType instrumentType,
        PortfolioId portfolioId,
        [EnumeratorCancellation] CancellationToken token)
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
                    BondId = await instrumentIdProvider.GetBondId(operation.InstrumentUid, token),
                    PortfolioId = portfolioId,
                    Quantity = -trade.Quantity,
                    Type = TradeOperationType.Sell,
                },
                Tinkoff.InvestApi.V1.InstrumentType.Share => new StockTradeOperation
                {
                    Id = trade.TradeId,
                    Date = trade.DateTime.ToDateTimeOffset(),
                    Amount = trade.Price.ToMoney(),
                    StockId = await instrumentIdProvider.GetStockId(operation.InstrumentUid, token),
                    PortfolioId = portfolioId,
                    Quantity = -trade.Quantity,
                    Type = TradeOperationType.Sell,
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}