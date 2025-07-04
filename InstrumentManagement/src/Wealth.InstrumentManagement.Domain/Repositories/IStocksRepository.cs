using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Domain.Repositories;

public interface IStocksRepository : IInstrumentsRepository
{
    Task ChangeDividend(InstrumentId id, Dividend dividend);
    Task<InstrumentId> CreateStock(string name, ISIN isin);
}