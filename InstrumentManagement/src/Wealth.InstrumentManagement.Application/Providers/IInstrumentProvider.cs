using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Providers;

public interface IInstrumentProvider
{
    public Task<Money> GetPrice(ISIN isin);
    public Task<string> GetName(ISIN isin);
    public Task<LotSize> GetLotSize(ISIN isin);
}