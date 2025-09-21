using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling;

public class BrokerFeeHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        InstrumentType instrumentType,
        PortfolioId portfolioId)
    {
        yield return instrumentType switch
        {
            InstrumentType.Bond => new BondBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = new Money(operation.Currency, operation.Payment),
                BondId = await instrumentIdProvider.GetBondIdByFigi(operation.Figi),
                PortfolioId = portfolioId,
            },
            InstrumentType.Share => new StockBrokerFeeOperation
            {
                Id = operation.Id,
                Date = operation.Date.ToDateTimeOffset(),
                Amount = new Money(operation.Currency, operation.Payment),
                StockId = await instrumentIdProvider.GetStockIdByFigi(operation.Figi),
                PortfolioId = portfolioId,
            },
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}