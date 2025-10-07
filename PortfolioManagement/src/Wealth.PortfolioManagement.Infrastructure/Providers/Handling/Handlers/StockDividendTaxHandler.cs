using Tinkoff.InvestApi.V1;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public sealed class StockDividendTaxHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        InstrumentType instrumentType,
        PortfolioId portfolioId)
    {
        if (instrumentType != InstrumentType.Share)
            throw new ArgumentOutOfRangeException(nameof(instrumentType));

        yield return new StockDividendTaxOperation
        {
            Id = operation.Id,
            Date = operation.Date.ToDateTimeOffset(),
            Amount = operation.Payment.ToMoney(),
            PortfolioId = portfolioId,
            StockId = await instrumentIdProvider.GetStockIdByFigi(operation.Figi),
        };
    }
}