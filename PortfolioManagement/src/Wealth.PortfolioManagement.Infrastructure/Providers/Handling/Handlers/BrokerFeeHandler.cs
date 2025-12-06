using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public class BrokerFeeHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        Tinkoff.InvestApi.V1.InstrumentType instrumentType,
        PortfolioId portfolioId)
    {
        yield return instrumentType switch
        {
            Tinkoff.InvestApi.V1.InstrumentType.Bond => new BondBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = operation.Payment.ToMoney(),
                BondId = await instrumentIdProvider.GetBondIdByFigi(operation.Figi),
                PortfolioId = portfolioId,
            },
            Tinkoff.InvestApi.V1.InstrumentType.Share => new StockBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = operation.Payment.ToMoney(),
                StockId = await instrumentIdProvider.GetStockIdByFigi(operation.Figi),
                PortfolioId = portfolioId,
            },
            Tinkoff.InvestApi.V1.InstrumentType.Currency => new CurrencyBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = operation.Payment.ToMoney(),
                CurrencyId = await instrumentIdProvider.GetCurrencyIdByFigi(operation.Figi),
                PortfolioId = portfolioId,
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}