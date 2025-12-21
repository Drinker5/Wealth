using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public interface IInstrumentIdProvider
{
    ValueTask<StockId> GetStockId(InstrumentId instrumentId);

    ValueTask<BondId> GetBondId(InstrumentId instrumentId);

    ValueTask<CurrencyId> GetCurrencyId(InstrumentId instrumentId);
}