using System.Runtime.CompilerServices;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public class BrokerFeeHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        Tinkoff.InvestApi.V1.InstrumentType instrumentType,
        PortfolioId portfolioId,
        [EnumeratorCancellation] CancellationToken token)
    {
        yield return instrumentType switch
        {
            Tinkoff.InvestApi.V1.InstrumentType.Bond => new BondBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = operation.Payment.ToMoney(),
                BondId = await instrumentIdProvider.GetBondId(operation.InstrumentUid, token),
                PortfolioId = portfolioId,
            },
            Tinkoff.InvestApi.V1.InstrumentType.Share => new StockBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = operation.Payment.ToMoney(),
                StockId = await instrumentIdProvider.GetStockId(operation.InstrumentUid, token),
                PortfolioId = portfolioId,
            },
            Tinkoff.InvestApi.V1.InstrumentType.Currency => new CurrencyBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = operation.Payment.ToMoney(),
                CurrencyId = await instrumentIdProvider.GetCurrencyId(operation.InstrumentUid, token),
                PortfolioId = portfolioId,
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}