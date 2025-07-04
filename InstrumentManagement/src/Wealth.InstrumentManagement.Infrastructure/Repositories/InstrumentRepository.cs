using System.Data;
using Dapper;
using Dommel;
using Wealth.InstrumentManagement.Domain;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class InstrumentRepository :
    IInstrumentsRepository,
    IBondsRepository,
    IStocksRepository, IDisposable
{
    private readonly WealthDbContext dbContext;
    private readonly IDbConnection connection;

    public InstrumentRepository(WealthDbContext dbContext)
    {
        this.dbContext = dbContext;
        connection = dbContext.CreateConnection();
    }

    public async Task<IEnumerable<Instrument>> GetInstruments()
    {
        return await connection.QueryAsync<Instrument>("SELECT * FROM Instruments");
    }

    public async Task<Instrument?> GetInstrument(InstrumentId instrumentId)
    {
        return await connection.QueryFirstOrDefaultAsync<Instrument>(
            "SELECT * FROM Instruments WHERE Id = @Id",
            new { Id = instrumentId.Id });
    }

    public async Task DeleteInstrument(InstrumentId instrumentId)
    {
        await connection.ExecuteAsync("DELETE FROM Instruments WHERE Id = @Id", new { Id = instrumentId.Id });
    }

    public async Task ChangePrice(InstrumentId id, Money price)
    {
        var instrument = await GetInstrument(id);
        if (instrument == null)
            return;

        instrument.ChangePrice(price);
        await connection.UpdateAsync(instrument);
    }

    public async Task<InstrumentId> CreateBond(string name, ISIN isin)
    {
        var bondInstrument = BondInstrument.Create(name, isin);
        var id = await connection.InsertAsync(bondInstrument);
        return (InstrumentId)id;
    }

    public async Task ChangeCoupon(InstrumentId id, Coupon coupon)
    {
        var bondInstrument = await GetInstrument(id) as BondInstrument;
        if (bondInstrument == null)
            return;

        bondInstrument.ChangeCoupon(coupon);
        await connection.UpdateAsync(bondInstrument);
    }

    public async Task ChangeDividend(InstrumentId id, Dividend dividend)
    {
        var stockInstrument = await GetInstrument(id) as StockInstrument;
        if (stockInstrument == null)
            return;

        stockInstrument.ChangeDividend(dividend);
        await connection.UpdateAsync(stockInstrument);
    }

    public async Task<InstrumentId> CreateStock(string name, ISIN isin)
    {
        var stockInstrument = StockInstrument.Create(name, isin);
        var id = await connection.InsertAsync(stockInstrument);
        return (InstrumentId)id;
    }

    public void Dispose()
    {
        connection.Dispose();
    }
}