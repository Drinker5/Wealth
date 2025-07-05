using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Domain.Repositories;

public interface IBondsRepository : IInstrumentsRepository
{
    Task<InstrumentId> CreateBond(string name, ISIN isin);
    Task ChangeCoupon(InstrumentId id, Coupon coupon);
}