using System.Data;
using Dapper;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class StocksRepository : IStocksRepository
{
    private readonly WealthDbContext dbContext;

    private readonly IDbConnection connection;

    public StocksRepository(WealthDbContext dbContext)
    {
        this.dbContext = dbContext;
        connection = dbContext.CreateConnection();
    }

    public async Task<IReadOnlyCollection<Stock>> GetStocks()
    {
        const string sql = """SELECT * FROM "GetStocks" LIMIT 10""";
        return await GetStocks(sql);
    }

    public async Task<Stock?> GetStock(ISIN isin)
    {
        const string sql = """SELECT * FROM "Stocks" WHERE "ISIN" = @isin""";
        var instruments = await GetStocks(sql, new { isin = isin.Value });
        return instruments.FirstOrDefault();
    }

    public async Task DeleteStock(StockId id)
    {
        const string sql = """DELETE FROM "Stocks" WHERE "Id" = @Id""";
        await connection.ExecuteAsync(sql, new { Id = id.Id });
    }

    public async Task ChangePrice(StockId id, Money price)
    {
        var instrument = await GetStock(id);
        if (instrument == null)
            return;

        instrument.ChangePrice(price);
        const string sql = """
                           UPDATE "Stocks" 
                           SET "Price_CurrencyId" = @CurrencyId, "Price_Amount" = @Amount
                           WHERE "Id" = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Id,
            CurrencyId = instrument.Price.CurrencyId.Code,
            Amount = instrument.Price.Amount,
        });
        dbContext.AddEvents(instrument);
    }

    public Task<StockId> CreateStock(StockId id, string name, ISIN isin)
    {
        var stockInstrument = Stock.Create(id, name, isin);
        return CreateStock(stockInstrument);
    }

    public async Task<Stock?> GetStock(StockId id)
    {
        const string sql = """SELECT * FROM "Stocks" WHERE "Id" = @Id""";
        var instruments = await GetStocks(sql, new { Id = id.Id });
        return instruments.FirstOrDefault();
    }

    public Task<StockId> CreateStock(string name, ISIN isin)
    {
        var stockInstrument = Stock.Create(name, isin);
        return CreateStock(stockInstrument);
    }

    private async Task<StockId> CreateStock(Stock stock)
    {
        const string sql = """
                           INSERT INTO "Stocks" ("Id", "Name", "ISIN") 
                           VALUES (@Id, @Name, @ISIN)
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = stock.Id.Id,
            Name = stock.Name,
            ISIN = stock.ISIN.Value,
        });
        dbContext.AddEvents(stock);

        return stock.Id;
    }

    public async Task ChangeDividend(StockId id, Dividend dividend)
    {
        var instrument = await GetStock(id);
        if (instrument == null)
            return;

        instrument.ChangeDividend(dividend);
        const string sql = """
                           UPDATE "Stocks" 
                           SET "Dividend_CurrencyId" = @CurrencyId, "Dividend_Amount" = @Amount
                           WHERE "Id" = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Id,
            CurrencyId = dividend.ValuePerYear.CurrencyId.Code,
            Amount = dividend.ValuePerYear.Amount,
        });
        dbContext.AddEvents(instrument);
    }

    public async Task ChangeLotSize(StockId id, int lotSize)
    {
        var instrument = await GetStock(id);
        if (instrument == null)
            return;

        instrument.ChangeLotSize(lotSize);
        const string sql = """
                           UPDATE "Stocks" 
                           SET "LotSize" = @LotSize
                           WHERE "Id" = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Id,
            LotSize = lotSize,
        });
        dbContext.AddEvents(instrument);
    }

    private enum Columns
    {
        Id = 1,
        ISIN,
        Name,
        Price_CurrencyId,
        Price_Amount,
        Dividend_CurrencyId,
        Dividend_Amount,
        LotSize
    }

    private async Task<IReadOnlyCollection<Stock>> GetStocks(string sql, object? param = null)
    {
        using var reader = await connection.ExecuteReaderAsync(sql, param);

        var instruments = new List<Stock>();
        while (reader.Read())
        {
            var stock = new Stock(reader.GetInt32((int)Columns.Id));
            if (!reader.IsDBNull((int)Columns.Dividend_CurrencyId))
            {
                stock.Dividend = new Dividend(
                    reader.GetString((int)Columns.Dividend_CurrencyId),
                    reader.GetDecimal((int)Columns.Dividend_Amount));
            }

            stock.LotSize = reader.GetInt32((int)Columns.LotSize);
            stock.Name = reader.GetString((int)Columns.Name);
            stock.ISIN = reader.GetString((int)Columns.ISIN);
            if (!reader.IsDBNull((int)Columns.Price_CurrencyId))
            {
                stock.Price = new Money(
                    reader.GetString((int)Columns.Price_CurrencyId),
                    reader.GetDecimal((int)Columns.Price_Amount));
            }

            instruments.Add(stock);
        }

        return instruments;
    }
}