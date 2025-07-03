using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Domain.Repositories;

public interface IBondsRepository
{
    Task<InstrumentId> CreateBond(string name, ISIN isin);
    Task ChangeCoupon(InstrumentId id, Coupon coupon);
}