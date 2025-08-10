// using Wealth.BuildingBlocks.Domain.Common;
// using Wealth.InstrumentManagement.Domain.Instruments;
// using Wealth.InstrumentManagement.Domain.Repositories;
//
// namespace Wealth.InstrumentManagement.Infrastructure.Repositories;
//
// public class InMemoryInstrumentRepository :
//     IBondsRepository,
//     IStocksRepository
// {
//     private List<Instrument> instruments = [];
//
//     public Task<IEnumerable<Instrument>> GetInstruments()
//     {
//         return Task.FromResult<IEnumerable<Instrument>>(instruments);
//     }
//
//     public Task<Instrument?> GetInstrument(InstrumentId instrumentId)
//     {
//         return Task.FromResult(instruments.FirstOrDefault(i => i.Id == instrumentId));
//     }
//
//     public Task<Instrument?> GetInstrument(ISIN isin)
//     {
//         return Task.FromResult(instruments.FirstOrDefault(i => i.ISIN == isin));
//     }
//
//     public Task DeleteInstrument(InstrumentId instrumentId)
//     {
//         instruments.RemoveAll(i => i.Id == instrumentId);
//         return Task.CompletedTask;
//     }
//
//     public async Task ChangePrice(InstrumentId id, Money price)
//     {
//         var instrument = await GetInstrument(id);
//         if (instrument == null)
//             return;
//
//         instrument.ChangePrice(price);
//     }
//
//     public Task<IReadOnlyCollection<Bond>> GetBonds()
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<Bond?> GetBond(BondId id)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<Bond?> GetBond(ISIN isin)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task DeleteBond(BondId id)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task ChangePrice(BondId id, Money price)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<BondId> CreateBond(BondId id, string name, ISIN isin)
//     {
//         throw new NotImplementedException();
//     }
//
//     Task<BondId> IBondsRepository.CreateBond(string name, ISIN isin)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task ChangeCoupon(BondId id, Coupon coupon)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<InstrumentId> CreateBond(string name, ISIN isin)
//     {
//         var bondInstrument = Bond.Create(name, isin);
//         instruments.Add(bondInstrument);
//         return Task.FromResult(bondInstrument.Id);
//     }
//
//     public Task<InstrumentId> CreateBond(InstrumentId id, string name, ISIN isin)
//     {
//         var bondInstrument = Bond.Create(id, name, isin);
//         instruments.Add(bondInstrument);
//         return Task.FromResult(bondInstrument.Id);
//     }
//
//     public async Task ChangeCoupon(InstrumentId id, Coupon coupon)
//     {
//         var bondInstrument = await GetInstrument(id) as Bond;
//         if (bondInstrument == null)
//             return;
//
//         bondInstrument.ChangeCoupon(coupon);
//     }
//
//     public async Task ChangeDividend(InstrumentId id, Dividend dividend)
//     {
//         var stockInstrument = await GetInstrument(id) as Stock;
//         if (stockInstrument == null)
//             return;
//
//         stockInstrument.ChangeDividend(dividend);
//     }
//
//     public Task<IEnumerable<Stock>> GetStocks()
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<Stock?> GetStock(StockId id)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<Stock?> GetStock(ISIN isin)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task DeleteStock(StockId id)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task ChangePrice(StockId id, Money price)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task ChangeDividend(StockId id, Dividend dividend)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<StockId> CreateStock(StockId id, string name, ISIN isin)
//     {
//         throw new NotImplementedException();
//     }
//
//     Task<StockId> IStocksRepository.CreateStock(string name, ISIN isin)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task ChangeLotSize(StockId id, int lotSize)
//     {
//         throw new NotImplementedException();
//     }
//
//     public Task<InstrumentId> CreateStock(string name, ISIN isin)
//     {
//         var stockInstrument = Stock.Create(name, isin);
//         instruments.Add(stockInstrument);
//         return Task.FromResult(stockInstrument.Id);
//     }
//
//     public Task<InstrumentId> CreateStock(InstrumentId id, string name, ISIN isin)
//     {
//         var stockInstrument = Stock.Create(id, name, isin);
//         instruments.Add(stockInstrument);
//         return Task.FromResult(stockInstrument.Id);
//     }
//
//     public async Task ChangeLotSize(InstrumentId id, int lotSize)
//     {
//         var stockInstrument = await GetInstrument(id) as Stock;
//         if (stockInstrument == null)
//             return;
//
//         stockInstrument.ChangeLotSize(lotSize);
//     }
// }