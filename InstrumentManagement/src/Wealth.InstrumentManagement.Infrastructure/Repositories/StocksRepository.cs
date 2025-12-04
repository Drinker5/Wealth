using System.Data;
using Dapper;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class StocksRepository(WealthDbContext dbContext) : IStocksRepository
{
    private readonly IDbConnection connection = dbContext.CreateConnection();

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

    public async Task<Stock?> GetStock(FIGI figi)
    {
        const string sql = """SELECT * FROM "Stocks" WHERE "FIGI" = @figi""";
        var instruments = await GetStocks(sql, new { figi = figi.Value });
        return instruments.FirstOrDefault();
    }

    public async Task DeleteStock(StockId id)
    {
        const string sql = """DELETE FROM "Stocks" WHERE "Id" = @Id""";
        await connection.ExecuteAsync(sql, new { Id = id.Value });
    }

    public async Task ChangePrice(StockId id, Money price)
    {
        var instrument = await GetStock(id);
        if (instrument == null)
            return;

        instrument.ChangePrice(price);
        const string sql = """
                           UPDATE "Stocks" 
                           SET "Price_Currency" = @Currency, "Price_Amount" = @Amount
                           WHERE "Id" = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            Currency = instrument.Price.Currency,
            Amount = instrument.Price.Amount,
        });
        dbContext.AddEvents(instrument);
    }

    public async Task<Stock?> GetStock(StockId id)
    {
        const string sql = """SELECT * FROM "Stocks" WHERE "Id" = @Id""";
        var instruments = await GetStocks(sql, new { Id = id.Value });
        return instruments.FirstOrDefault();
    }

    public async Task<StockId> CreateStock(
        string index,
        string name,
        ISIN isin,
        FIGI figi,
        LotSize lotSize,
        CancellationToken token = default)
    {
        const string sql = """SELECT nextval('"StocksHiLo"')""";
        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: token);

        var nextId = await connection.ExecuteScalarAsync<int>(command);
        var stock = Stock.Create(new StockId(nextId), index, name, isin, figi);
        stock.ChangeLotSize(lotSize);
        return await CreateStock(stock);
    }

    private async Task<StockId> CreateStock(Stock stock)
    {
        const string sql = """
                           INSERT INTO "Stocks" ("Id", index, "Name", "ISIN", "FIGI", "LotSize") 
                           VALUES (@Id, @Index, @Name, @ISIN, @FIGI, @LotSize)
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = stock.Id.Value,
            Index = stock.Index,
            Name = stock.Name,
            ISIN = stock.Isin.Value,
            FIGI = stock.Figi.Value,
            LotSize = stock.LotSize.Value,
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
                           SET "Dividend_Currency" = @Currency, "Dividend_Amount" = @Amount
                           WHERE "Id" = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            Currency = dividend.ValuePerYear.Currency,
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
            Id = id.Value,
            LotSize = lotSize,
        });
        dbContext.AddEvents(instrument);
    }

    public async Task ChangeIndex(StockId id, string index)
    {
        var instrument = await GetStock(id);
        if (instrument == null)
            return;
        
        instrument.ChangeIndex(index);
        const string sql = """
                           UPDATE "Stocks" 
                           SET index = @Index
                           WHERE "Id" = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            Index = index,
        });
        dbContext.AddEvents(instrument);
    }

    private enum Columns
    {
        Id = 0,
        Name,
        ISIN,
        Price_Amount,
        Dividend_Amount,
        LotSize,
        FIGI,
        Price_Currency,
        Dividend_Currency,
    }

    private async Task<IReadOnlyCollection<Stock>> GetStocks(string sql, object? param = null)
    {
        using var reader = await connection.ExecuteReaderAsync(sql, param);

        var instruments = new List<Stock>();
        while (reader.Read())
        {
            var stock = new Stock(reader.GetInt32((int)Columns.Id));
            if (!reader.IsDBNull((int)Columns.Dividend_Currency))
            {
                stock.Dividend = new Dividend(
                    (CurrencyCode)reader.GetByte((int)Columns.Dividend_Currency),
                    reader.GetDecimal((int)Columns.Dividend_Amount));
            }

            stock.LotSize = reader.GetInt32((int)Columns.LotSize);
            stock.Name = reader.GetString((int)Columns.Name);
            stock.Isin = reader.GetString((int)Columns.ISIN);
            stock.Figi = reader.GetString((int)Columns.FIGI);
            if (!reader.IsDBNull((int)Columns.Price_Currency))
            {
                stock.Price = new Money(
                    (CurrencyCode)reader.GetByte((int)Columns.Price_Currency),
                    reader.GetDecimal((int)Columns.Price_Amount));
            }

            instruments.Add(stock);
        }

        return instruments;
    }
}