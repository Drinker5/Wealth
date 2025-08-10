using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Domain.Repositories;

public interface IBondsRepository
{
    Task<IReadOnlyCollection<Bond>> GetBonds();
    Task<Bond?> GetBond(BondId id);
    Task<Bond?> GetBond(ISIN isin);
    Task DeleteBond(BondId id);
    Task ChangePrice(BondId id, Money price);
    Task<BondId> CreateBond(BondId id, string name, ISIN isin);
    Task<BondId> CreateBond(string name, ISIN isin);
    Task ChangeCoupon(BondId id, Coupon coupon);
}