using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Domain.Repositories;

public interface IInstrumentsRepository
{
    Task<IEnumerable<Instrument>> GetInstruments();
    Task<Instrument?> GetInstrument(InstrumentId instrumentId);
    Task<Instrument?> GetInstrument(ISIN isin);
    Task DeleteInstrument(InstrumentId instrumentId);
    Task ChangePrice(InstrumentId id, Money price);
}