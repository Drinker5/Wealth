using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Domain.Repositories;

public interface IInstrumentsRepository
{
    Task<IEnumerable<Instrument>> GetInstruments();
    Task<Instrument?> GetInstrument(InstrumentId id);
    Task CreateBond(string name, ISIN isin);
    Task CreateStock(string name, ISIN isin);
    Task DeleteInstrument(InstrumentId instrumentId);
    Task ChangeDividend(InstrumentId id, Dividend dividend);
    Task ChangeCoupon(InstrumentId id, Coupon coupon);
    Task ChangePrice(InstrumentId id, Money price);
}