using System.Data;
using Dapper;
using SharpJuice.Essentials;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.BuildingBlocks.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class StocksRepository(
    IConnectionFactory connectionFactory,
    IEventTracker eventTracker,
    IClock clock) : IStocksRepository
{
    private readonly IDbConnection connection = connectionFactory.CreateConnection();

    private const string COLUMNS =
        """
        "Id",
        "Name",
        "ISIN",
        "Price_Amount",
        "Dividend_Amount",
        "LotSize",
        "FIGI",
        "Price_Currency",
        "Dividend_Currency",
        ticker,
        instrument_id
        """;

    private enum Columns : byte
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
        Ticker,
        InstrumentId
    }

    public async Task<IReadOnlyCollection<Stock>> GetStocks()
    {
        // language=postgresql
        const string sql = $"""SELECT {COLUMNS} FROM "GetStocks";""";
        return await GetStocks(sql);
    }

    public async Task<Stock?> GetStock(ISIN isin)
    {
        // language=postgresql
        const string sql = $"""SELECT {COLUMNS} FROM "Stocks" WHERE "ISIN" = @isin""";
        var instruments = await GetStocks(sql, new { isin = isin.Value });
        return instruments.FirstOrDefault();
    }

    public async Task<Stock?> GetStock(FIGI figi)
    {
        // language=postgresql
        const string sql = $"""SELECT {COLUMNS} FROM "Stocks" WHERE "FIGI" = @figi""";
        var instruments = await GetStocks(sql, new { figi = figi.Value });
        return instruments.FirstOrDefault();
    }

    public async Task<Stock?> GetStock(InstrumentUId uId)
    {
        // language=postgresql
        const string sql = $"""SELECT {COLUMNS} FROM "Stocks" WHERE instrument_id = @instrumentId""";
        var instruments = await GetStocks(sql, new { instrumentId = uId.Value });
        return instruments.FirstOrDefault();
    }

    public async Task DeleteStock(StockId id)
    {
        // language=postgresql
        const string sql = """DELETE FROM "Stocks" WHERE "Id" = @Id""";
        await connection.ExecuteAsync(sql, new { Id = id.Value });
    }

    public async Task ChangePrice(StockId id, Money price)
    {
        var instrument = await GetStock(id);
        if (instrument == null)
            return;

        instrument.ChangePrice(price);
        const string sql =
            // language=postgresql
            """
            UPDATE "Stocks" 
            SET "Price_Currency" = @Currency, 
                "Price_Amount" = @Amount,
                price_updated_at = @Now
            WHERE "Id" = @Id
            """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            Currency = instrument.Price.Currency,
            Amount = instrument.Price.Amount,
            Now = clock.Now 
        });
        eventTracker.AddEvents(instrument);
    }

    public async Task<Stock?> GetStock(StockId id)
    {
        // language=postgresql
        const string sql = $"""SELECT {COLUMNS} FROM "Stocks" WHERE "Id" = @Id""";
        var instruments = await GetStocks(sql, new { Id = id.Value });
        return instruments.FirstOrDefault();
    }

    private async Task<Stock?> GetStock(ISIN isin, FIGI figi, InstrumentUId instrumentUId, CancellationToken token)
    {
        var instruments = await GetStocks(
            // language=postgresql
            $"""
             SELECT {COLUMNS} FROM "Stocks"
             WHERE "ISIN" = @Isin
                 OR "FIGI" = @Figi
                 OR instrument_id = @InstrumentId
             """,
            new
            {
                Isin = isin.Value,
                Figi = figi.Value,
                InstrumentId = instrumentUId.Value
            });

        if (instruments.Count > 1)
            throw new InvalidOperationException($"Found more than one stock Isin: ${isin.Value}, Figi: ${figi.Value}, InstrumentId: ${instrumentUId.Value}");

        return instruments.FirstOrDefault();
    }

    public async Task<StockId> CreateStock(
        CreateStockCommand command,
        CancellationToken token = default)
    {
        // language=postgresql
        const string sql = """SELECT nextval('"StocksHiLo"')""";
        var nextId = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            commandText: sql,
            cancellationToken: token));

        var stock = Stock.Create(new StockId(nextId), command.Ticker, command.Name, command.Isin, command.Figi, command.InstrumentUId);
        stock.ChangeLotSize(command.LotSize);
        return await CreateStock(stock);
    }

    public async Task<StockId> UpsertStock(CreateStockCommand command, CancellationToken token = default)
    {
        var stock = await GetStock(command.Isin, command.Figi, command.InstrumentUId, token);
        if (stock == null)
            return await CreateStock(command, token);

        await UpdateStock(stock, command, token);
        return stock.Id;
    }

    private async Task UpdateStock(Stock instrument, CreateStockCommand command, CancellationToken token)
    {
        instrument.ChangeName(command.Name);
        instrument.ChangeLotSize(command.LotSize);
        instrument.ChangeTicker(command.Ticker);
        instrument.ChangeIsin(command.Isin);
        instrument.ChangeFigi(command.Figi);
        instrument.ChangeInstrumentId(command.InstrumentUId);
        await connection.ExecuteAsync(
            // language=postgresql
            """
            UPDATE "Stocks" 
            SET "Name" = @Name, "LotSize" = @LotSize, ticker = @Ticker, "ISIN" = @Isin, "FIGI" = @Figi, instrument_id = @InstrumentId
            WHERE "Id" = @Id
            """,
            new
            {
                Id = instrument.Id.Value,
                Name = command.Name,
                LotSize = command.LotSize.Value,
                Ticker = command.Ticker.Value,
                ISIN = command.Isin.Value,
                Figi = command.Figi.Value,
                InstrumentId = command.InstrumentUId.Value
            });
        eventTracker.AddEvents(instrument);
    }

    private async Task<StockId> CreateStock(Stock stock)
    {
        const string sql =
            // language=postgresql
            """
            INSERT INTO "Stocks" ("Id", ticker, "Name", "ISIN", "FIGI", "LotSize", instrument_id) 
            VALUES (@Id, @Ticker, @Name, @ISIN, @FIGI, @LotSize, @InstrumentId)
            """;

        await connection.ExecuteAsync(sql, new
        {
            Id = stock.Id.Value,
            Ticker = stock.Ticker.Value,
            Name = stock.Name,
            ISIN = stock.Isin.Value,
            FIGI = stock.Figi.Value,
            LotSize = stock.LotSize.Value,
            InstrumentId = stock.InstrumentUId.Value
        });
        eventTracker.AddEvents(stock);

        return stock.Id;
    }

    public async Task ChangeDividend(StockId id, Dividend dividend)
    {
        var instrument = await GetStock(id);
        if (instrument == null)
            return;

        instrument.ChangeDividend(dividend);
        const string sql =
            // language=postgresql
            """
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
        eventTracker.AddEvents(instrument);
    }

    public async Task ChangeLotSize(StockId id, int lotSize)
    {
        var instrument = await GetStock(id);
        if (instrument == null)
            return;

        instrument.ChangeLotSize(lotSize);
        const string sql =
            // language=postgresql
            """
            UPDATE "Stocks" 
            SET "LotSize" = @LotSize
            WHERE "Id" = @Id
            """;

        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            LotSize = lotSize,
        });
        eventTracker.AddEvents(instrument);
    }

    public async Task ChangeTicker(StockId id, Ticker ticker)
    {
        var instrument = await GetStock(id);
        if (instrument == null)
            return;

        instrument.ChangeTicker(ticker);
        const string sql =
            // language=postgresql
            """
            UPDATE "Stocks" 
            SET ticker = @Ticker
            WHERE "Id" = @Id
            """;

        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            Ticker = ticker,
        });
        eventTracker.AddEvents(instrument);
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
            stock.InstrumentUId = reader.GetGuid((int)Columns.InstrumentId);
            stock.Ticker = reader.GetString((int)Columns.Ticker);
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