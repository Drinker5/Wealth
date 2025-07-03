using Wealth.InstrumentManagement.Domain;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class InMemoryInstrumentRepository :
    IInstrumentsRepository,
    IBondsRepository,
    IStocksRepository
{
    private List<Instrument> instruments = [];

    public InMemoryInstrumentRepository()
    {
    }

    public Task<IEnumerable<Instrument>> GetInstruments()
    {
        return Task.FromResult<IEnumerable<Instrument>>(instruments);
    }

    public Task<Instrument?> GetInstrument(InstrumentId id)
    {
        return Task.FromResult(instruments.FirstOrDefault(i => i.Id == id));
    }

    public Task DeleteInstrument(InstrumentId instrumentId)
    {
        instruments.RemoveAll(i => i.Id == instrumentId);
        return Task.CompletedTask;
    }

    public async Task ChangePrice(InstrumentId id, Money price)
    {
        var instrument = await GetInstrument(id);
        if (instrument == null)
            return;

        instrument.ChangePrice(price);
    }

    public Task<InstrumentId> CreateBond(string name, ISIN isin)
    {
        var bondInstrument = BondInstrument.Create(name, isin);
        instruments.Add(bondInstrument);
        return Task.FromResult(bondInstrument.Id);
    }

    public async Task ChangeCoupon(InstrumentId id, Coupon coupon)
    {
        var bondInstrument = await GetInstrument(id) as BondInstrument;
        if (bondInstrument == null)
            return;

        bondInstrument.ChangeCoupon(coupon);
    }

    public async Task ChangeDividend(InstrumentId id, Dividend dividend)
    {
        var stockInstrument = await GetInstrument(id) as StockInstrument;
        if (stockInstrument == null)
            return;

        stockInstrument.ChangeDividend(dividend);
    }

    public Task<InstrumentId> CreateStock(string name, ISIN isin)
    {
        var stockInstrument = StockInstrument.Create(name, isin);
        instruments.Add(stockInstrument);
        return Task.FromResult(stockInstrument.Id);
    }
}