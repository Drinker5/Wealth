using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.Providers;

public interface IInstrumentIdProvider
{
    ValueTask<StockId> GetStockIdByFigi(string figi);

    ValueTask<BondId> GetBondIdByFigi(string figi);
}