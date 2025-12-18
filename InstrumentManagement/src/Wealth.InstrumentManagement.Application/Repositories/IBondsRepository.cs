using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Repositories;

public interface IBondsRepository
{
    Task<IReadOnlyCollection<Bond>> GetBonds();
    Task<Bond?> GetBond(BondId id);
    Task<Bond?> GetBond(ISIN isin);
    Task<Bond?> GetBond(FIGI figi);
    Task DeleteBond(BondId id);
    Task ChangePrice(BondId id, Money price);
    Task<BondId> CreateBond(string name, ISIN isin, FIGI figi, CancellationToken token);
    Task ChangeCoupon(BondId id, Coupon coupon);
}