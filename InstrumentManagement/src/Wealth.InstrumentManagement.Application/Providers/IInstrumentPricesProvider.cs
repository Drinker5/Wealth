using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Providers;

public interface IInstrumentPricesProvider
{
    Task<IReadOnlyDictionary<InstrumentUId, decimal>> ProvidePrices(
        IReadOnlyCollection<InstrumentUId> instrumentUIds,
        CancellationToken token);
}