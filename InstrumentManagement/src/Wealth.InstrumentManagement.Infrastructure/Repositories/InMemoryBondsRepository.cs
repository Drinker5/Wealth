using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class InMemoryBondsRepository : IBondsRepository
{
    private List<Bond> bonds = [];
    private static int currentId = 0;

    public Task<IReadOnlyCollection<Bond>> GetBonds()
    {
        return Task.FromResult<IReadOnlyCollection<Bond>>(bonds);
    }

    public Task<Bond?> GetBond(BondId id)
    {
        return Task.FromResult(bonds.FirstOrDefault(b => b.Id == id));
    }

    public Task<Bond?> GetBond(ISIN isin)
    {
        return Task.FromResult(bonds.FirstOrDefault(b => b.ISIN == isin));
    }

    public Task DeleteBond(BondId id)
    {
        bonds.RemoveAll(i => i.Id == id);
        return Task.CompletedTask;
    }

    public async Task ChangePrice(BondId id, Money price)
    {
        var bond = await GetBond(id);
        if (bond != null) bond.Price = price;
    }

    public Task<BondId> CreateBond(string name, ISIN isin, CancellationToken token = default)
    {
        var bondId = Interlocked.Increment(ref currentId);
        var bond = Bond.Create(bondId, name, isin);
        return Task.FromResult(bond.Id);
    }

    public async Task ChangeCoupon(BondId id, Coupon coupon)
    {
        var bond = await GetBond(id);
        if (bond != null) bond.Coupon = coupon;
    }
}