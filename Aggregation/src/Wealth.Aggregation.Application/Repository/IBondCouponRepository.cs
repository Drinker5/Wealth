using Wealth.Aggregation.Application.Commands;

namespace Wealth.Aggregation.Application.Repository;

public interface IBondCouponRepository
{
    Task Upsert(BondCoupon operation, CancellationToken token);
}