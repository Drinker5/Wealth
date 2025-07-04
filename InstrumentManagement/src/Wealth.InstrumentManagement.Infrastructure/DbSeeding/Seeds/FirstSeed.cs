using Dommel;
using Wealth.InstrumentManagement.Domain;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.DbSeeding.Seeds;

public class FirstSeed : IDbSeeder
{
    public async Task SeedAsync(WealthDbContext context)
    {
        var connection = context.CreateConnection();
        if (!await connection.AnyAsync<Instrument>())
            await connection.InsertAllAsync(GetInstruments());
    }

    private static IEnumerable<Instrument> GetInstruments()
    {
        var bond1 = BondInstrument.Create("test-bond-1", new ISIN("000000000001"));
        bond1.ChangePrice(new Money("FOO", 12.34m));
        bond1.ChangeCoupon(new Coupon("FOO", 33m));
        yield return bond1;
        
        var bond2 = BondInstrument.Create("test-bond-2", new ISIN("000000000002"));
        bond2.ChangePrice(new Money("BAR", 2.12m));
        bond2.ChangeCoupon(new Coupon("FOO", 44m));
        yield return bond2;
        
        var stock1 = StockInstrument.Create("test-stock-1", new ISIN("000000000003"));
        stock1.ChangePrice(new Money("FOO", 111m));
        stock1.ChangeDividend(new Dividend("BAR", 222m));
        yield return stock1;
        
        var stock2 = StockInstrument.Create("test-stock-2", new ISIN("000000000004"));
        stock2.ChangePrice(new Money("BAR", 222m));
        stock2.ChangeDividend(new Dividend("BAR", 333m));
        yield return stock2;
    }
}