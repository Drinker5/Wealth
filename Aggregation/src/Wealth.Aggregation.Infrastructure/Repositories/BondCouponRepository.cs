using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Commands;
using Wealth.Aggregation.Application.Repository;

namespace Wealth.Aggregation.Infrastructure.Repositories;

public class BondCouponRepository(ITableWriterBuilder tableWriterBuilder) : IBondCouponRepository
{
    private readonly ITableWriter<BondCoupon> _tableWriter = tableWriterBuilder
        .For<BondCoupon>("bond_coupon")
        .AddColumn("op_id", i => i.Id)
        .AddColumn("date", i => i.Date)
        .AddColumn("portfolio_id", i => i.PortfolioId.Value)
        .AddColumn("bond_id", i => i.BondId.Value)
        .AddColumn("amount", i => i.Amount.Amount)
        .AddColumn("currency", i => (byte)i.Amount.CurrencyId.Value)
        .Build();

    public async Task Upsert(BondCoupon operation, CancellationToken token)
    {
        await _tableWriter.Insert([operation], token);
    }
}