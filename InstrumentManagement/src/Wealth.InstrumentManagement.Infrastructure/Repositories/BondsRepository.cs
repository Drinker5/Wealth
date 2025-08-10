// using System.Data;
// using Dapper;
// using Wealth.BuildingBlocks.Domain.Common;
// using Wealth.InstrumentManagement.Domain.Instruments;
// using Wealth.InstrumentManagement.Domain.Repositories;
// using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;
//
// namespace Wealth.InstrumentManagement.Infrastructure.Repositories;
//
// public class BondsRepository : IBondsRepository
// {
//     private readonly WealthDbContext dbContext;
//     private readonly IDbConnection connection;
//
//     public BondsRepository(WealthDbContext dbContext)
//     {
//         this.dbContext = dbContext;
//         connection = dbContext.CreateConnection();
//     }
//
//     public async Task<IEnumerable<Instrument>> GetInstruments()
//     {
//         var sql = """SELECT * FROM "Instruments" LIMIT 10""";
//         return await GetInstruments(sql);
//     }
//
//     public async Task<Instrument?> GetInstrument(InstrumentId instrumentId)
//     {
//         var sql = """SELECT * FROM "Instruments" WHERE "Id" = @Id""";
//         var instruments = await GetInstruments(sql, new { Id = instrumentId.Id });
//         return instruments.FirstOrDefault();
//     }
//
//     public async Task<Instrument?> GetInstrument(ISIN isin)
//     {
//         var sql = """SELECT * FROM "Instruments" WHERE "ISIN" = @isin""";
//         var instruments = await GetInstruments(sql, new { isin = isin.Value });
//         return instruments.FirstOrDefault();
//     }
//
//     public async Task DeleteInstrument(InstrumentId instrumentId)
//     {
//         var sql = """DELETE FROM "Instruments" WHERE "Id" = @Id""";
//         await connection.ExecuteAsync(sql, new { Id = instrumentId.Id });
//     }
//
//     public async Task ChangePrice(InstrumentId id, Money price)
//     {
//         var instrument = await GetInstrument(id);
//         if (instrument == null)
//             return;
//
//         instrument.ChangePrice(price);
//         var sql = """
//                   UPDATE "Instruments" 
//                   SET "Price_CurrencyId" = @CurrencyId, "Price_Amount" = @Amount
//                   WHERE "Id" = @Id
//                   """;
//         await connection.ExecuteAsync(sql, new
//         {
//             Id = id.Id,
//             CurrencyId = instrument.Price.CurrencyId.Code,
//             Amount = instrument.Price.Amount,
//         });
//         dbContext.AddEvents(instrument);
//     }
//
//     public Task<InstrumentId> CreateBond(InstrumentId id, string name, ISIN isin)
//     {
//         var bondInstrument = Bond.Create(id, name, isin);
//         return CreateBond(bondInstrument);
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
//         return CreateBond(bondInstrument);
//     }
//
//     private async Task<InstrumentId> CreateBond(Bond bond)
//     {
//         var sql = """
//                   INSERT INTO "Instruments" ("Id", "Name", "ISIN", "Type") 
//                   VALUES (@Id, @Name, @ISIN, @Type)
//                   """;
//         await connection.ExecuteAsync(sql, new
//         {
//             Id = bond.Id.Id,
//             Name = bond.Name,
//             ISIN = bond.ISIN.Value,
//             Type = bond.Type
//         });
//         dbContext.AddEvents(bond);
//
//         return bond.Id;
//     }
//
//     public Task<InstrumentId> CreateStock(InstrumentId id, string name, ISIN isin)
//     {
//         var stockInstrument = Stock.Create(id, name, isin);
//         return CreateStock(stockInstrument);
//     }
//
//     public Task<InstrumentId> CreateStock(string name, ISIN isin)
//     {
//         var stockInstrument = Stock.Create(name, isin);
//         return CreateStock(stockInstrument);
//     }
//
//     private async Task<InstrumentId> CreateStock(Stock stock)
//     {
//         var sql = """
//                   INSERT INTO "Instruments" ("Id", "Name", "ISIN", "Type") 
//                   VALUES (@Id, @Name, @ISIN, @Type)
//                   """;
//         await connection.ExecuteAsync(sql, new
//         {
//             Id = stock.Id.Id,
//             Name = stock.Name,
//             ISIN = stock.ISIN.Value,
//             Type = stock.Type
//         });
//         dbContext.AddEvents(stock);
//
//         return stock.Id;
//     }
//
//     public async Task ChangeCoupon(InstrumentId id, Coupon coupon)
//     {
//         var instrument = await GetInstrument(id) as Bond;
//         if (instrument == null)
//             return;
//
//         instrument.ChangeCoupon(coupon);
//         var sql = """
//                   UPDATE "Instruments" 
//                   SET "Coupon_CurrencyId" = @CurrencyId, "Coupon_Amount" = @Amount
//                   WHERE "Id" = @Id
//                   """;
//         await connection.ExecuteAsync(sql, new
//         {
//             Id = id.Id,
//             CurrencyId = coupon.ValuePerYear.CurrencyId.Code,
//             Amount = coupon.ValuePerYear.Amount,
//         });
//         dbContext.AddEvents(instrument);
//     }
//
//     public async Task ChangeDividend(InstrumentId id, Dividend dividend)
//     {
//         var instrument = await GetInstrument(id) as Stock;
//         if (instrument == null)
//             return;
//
//         instrument.ChangeDividend(dividend);
//         var sql = """
//                   UPDATE "Instruments" 
//                   SET "Dividend_CurrencyId" = @CurrencyId, "Dividend_Amount" = @Amount
//                   WHERE "Id" = @Id
//                   """;
//         await connection.ExecuteAsync(sql, new
//         {
//             Id = id.Id,
//             CurrencyId = dividend.ValuePerYear.CurrencyId.Code,
//             Amount = dividend.ValuePerYear.Amount,
//         });
//         dbContext.AddEvents(instrument);
//     }
//
//     public async Task ChangeLotSize(InstrumentId id, int lotSize)
//     {
//         var instrument = await GetInstrument(id) as Stock;
//         if (instrument == null)
//             return;
//
//         instrument.ChangeLotSize(lotSize);
//         var sql = """
//                   UPDATE "Instruments" 
//                   SET "LotSize" = @LotSize
//                   WHERE "Id" = @Id
//                   """;
//         await connection.ExecuteAsync(sql, new
//         {
//             Id = id.Id,
//             LotSize = lotSize,
//         });
//         dbContext.AddEvents(instrument);
//     }
//
//     private async Task<IEnumerable<Instrument>> GetInstruments(string sql, object? param = null)
//     {
//         using var reader = await connection.ExecuteReaderAsync(sql, param);
//
//         var instruments = new List<Instrument>();
//         while (reader.Read())
//         {
//             var discriminator = (InstrumentType)reader.GetInt32(reader.GetOrdinal(nameof(Instrument.Type)));
//
//             Instrument? instrument = discriminator switch
//             {
//                 InstrumentType.Bond => BondParse(reader),
//                 InstrumentType.Stock => StockParse(reader),
//                 _ => null,
//             };
//
//             if (instrument == null)
//                 continue;
//
//             instrument.Name = reader.GetString(reader.GetOrdinal(nameof(instrument.Name)));
//             instrument.ISIN = reader.GetString(reader.GetOrdinal(nameof(instrument.ISIN)));
//             if (!reader.IsDBNull(reader.GetOrdinal("Price_CurrencyId")))
//             {
//                 instrument.Price = new Money(
//                     reader.GetString(reader.GetOrdinal("Price_CurrencyId")),
//                     reader.GetDecimal(reader.GetOrdinal("Price_Amount")));
//             }
//
//             instruments.Add(instrument);
//         }
//
//         return instruments;
//
//         Bond BondParse(IDataReader reader)
//         {
//             var bondInstrument = new Bond(reader.GetGuid(reader.GetOrdinal(nameof(Bond.Id))));
//             if (!reader.IsDBNull(reader.GetOrdinal("Coupon_CurrencyId")))
//             {
//                 bondInstrument.Coupon = new Coupon(
//                     reader.GetString(reader.GetOrdinal("Coupon_CurrencyId")),
//                     reader.GetDecimal(reader.GetOrdinal("Coupon_Amount")));
//             }
//
//             return bondInstrument;
//         }
//
//         Instrument? StockParse(IDataReader reader)
//         {
//             var stockInstrument = new Stock(reader.GetGuid(reader.GetOrdinal(nameof(Stock.Id))));
//             if (!reader.IsDBNull(reader.GetOrdinal("Dividend_CurrencyId")))
//             {
//                 stockInstrument.Dividend = new Dividend(
//                     reader.GetString(reader.GetOrdinal("Dividend_CurrencyId")),
//                     reader.GetDecimal(reader.GetOrdinal("Dividend_Amount")));
//             }
//
//             stockInstrument.LotSize = reader.GetInt32(reader.GetOrdinal(nameof(stockInstrument.LotSize)));
//             return stockInstrument;
//         }
//     }
// }