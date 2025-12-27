using System.Runtime.CompilerServices;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.Operations;
using Operation = Wealth.PortfolioManagement.Domain.Operations.Operation;

namespace Wealth.PortfolioManagement.Infrastructure.Providers.Handling.Handlers;

public sealed class StockDividendTaxHandler(IInstrumentIdProvider instrumentIdProvider) : IOperationHandler
{
    public async IAsyncEnumerable<Operation> Handle(
        Tinkoff.InvestApi.V1.Operation operation,
        Tinkoff.InvestApi.V1.InstrumentType instrumentType,
        PortfolioId portfolioId,
        [EnumeratorCancellation] CancellationToken token)
    {
        if (instrumentType != Tinkoff.InvestApi.V1.InstrumentType.Share)
            throw new ArgumentOutOfRangeException(nameof(instrumentType));

        yield return new StockDividendTaxOperation
        {
            Id = operation.Id,
            Date = operation.Date.ToDateTimeOffset(),
            Amount = operation.Payment.ToMoney(),
            PortfolioId = portfolioId,
            StockId = await instrumentIdProvider.GetStockId(operation.InstrumentUid, token),
        };
    }
}