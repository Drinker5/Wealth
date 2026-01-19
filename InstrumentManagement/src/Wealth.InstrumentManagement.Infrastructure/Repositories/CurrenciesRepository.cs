using System.Data;
using Dapper;
using SharpJuice.Essentials;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.BuildingBlocks.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class CurrenciesRepository(
    IConnectionFactory connectionFactory,
    IEventTracker eventTracker,
    IClock clock) : ICurrenciesRepository
{
    private readonly IDbConnection connection = connectionFactory.CreateConnection();

    public async Task<Currency?> GetCurrency(CurrencyId id)
    {
        // language=postgresql
        const string sql = "SELECT * FROM currencies WHERE id = @Id";
        var instruments = await GetCurrencies(sql, new { Id = id.Value });
        return instruments.FirstOrDefault();
    }

    public async Task<Currency?> GetCurrency(FIGI figi)
    {
        // language=postgresql
        const string sql = "SELECT * FROM currencies WHERE figi = @figi";
        var instruments = await GetCurrencies(sql, new { figi = figi.Value });
        return instruments.FirstOrDefault();
    }

    public async Task<Currency?> GetCurrency(InstrumentUId uId)
    {
        // language=postgresql
        const string sql = "SELECT * FROM currencies WHERE instrument_id = @instrumentId";
        var instruments = await GetCurrencies(sql, new { instrumentId = uId.Value });
        return instruments.FirstOrDefault();
    }

    public async Task DeleteCurrency(CurrencyId instrumentId)
    {
        // language=postgresql
        const string sql = "DELETE FROM currencies WHERE id = @Id";
        await connection.ExecuteAsync(sql, new { Id = instrumentId.Value });
    }

    public async Task ChangePrice(CurrencyId id, Money price)
    {
        var currency = await GetCurrency(id);
        if (currency == null)
            return;

        currency.ChangePrice(price);
        // language=postgresql
        const string sql = """
                           UPDATE currencies 
                           SET price_currency = @Currency, 
                               price_amount = @Amount,
                               price_updated_at = @Now
                           WHERE id = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            Currency = currency.Price.Currency,
            Amount = currency.Price.Amount,
            Now = clock.Now
        });
        eventTracker.AddEvents(currency);
    }

    public async Task<CurrencyId> CreateCurrency(CreateCurrencyCommand command, CancellationToken token = default)
    {
        // language=postgresql
        const string sql = "SELECT nextval('currencies_hilo')";
        var nextId = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            commandText: sql,
            cancellationToken: token));

        var currencyInstrument = Currency.Create(new CurrencyId(nextId), command.Name, command.Figi, command.InstrumentUId, command.Currency);
        return await CreateCurrency(currencyInstrument);
    }

    public async Task<CurrencyId> UpsertCurrency(CreateCurrencyCommand command, CancellationToken token)
    {
        var currency = await GetCurrency(command.Figi, command.InstrumentUId, token);
        if (currency == null)
            return await CreateCurrency(command, token);

        await UpdateCurrency(currency, command, token);
        return currency.Id;
    }

    private async Task<Currency?> GetCurrency(FIGI figi, InstrumentUId instrumentUId, CancellationToken token)
    {
        var instruments = await GetCurrencies(
            // language=postgresql
            """
            SELECT * FROM currencies
            WHERE figi = @Figi 
               OR instrument_id = @InstrumentId
            """,
            new
            {
                Figi = figi.Value,
                InstrumentId = instrumentUId.Value
            });

        if (instruments.Count > 1)
            throw new InvalidOperationException($"Found more than one currency Figi: ${figi.Value}, InstrumentId: ${instrumentUId.Value}");

        return instruments.FirstOrDefault();
    }

    private async Task UpdateCurrency(Currency instrument, CreateCurrencyCommand command, CancellationToken token)
    {
        instrument.ChangeName(command.Name);
        instrument.ChangeFigi(command.Figi);
        instrument.ChangeInstrumentId(command.InstrumentUId);
        await connection.ExecuteAsync(
            // language=postgresql
            """
            UPDATE currencies 
            SET name = @Name, figi = @Figi, instrument_id = @InstrumentId
            WHERE id = @Id
            """,
            new
            {
                Id = instrument.Id.Value,
                Name = command.Name,
                Figi = command.Figi.Value,
                InstrumentId = command.InstrumentUId.Value
            });
        eventTracker.AddEvents(instrument);
    }

    public Task<IReadOnlyCollection<Currency>> GetCurrencies()
    {
        // language=postgresql
        const string sql = "SELECT * FROM currencies LIMIT 10";
        return GetCurrencies(sql);
    }

    private async Task<CurrencyId> CreateCurrency(Currency currency)
    {
        // language=postgresql
        const string sql = """
                           INSERT INTO currencies (id, name, figi, instrument_id, price_currency) 
                           VALUES (@Id, @Name, @FIGI, @InstrumentId, @PriceCurrency)
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = currency.Id.Value,
            Name = currency.Name,
            FIGI = currency.Figi.Value,
            InstrumentId = currency.InstrumentUId.Value,
            PriceCurrency = currency.Price.Currency
        });
        eventTracker.AddEvents(currency);

        return currency.Id;
    }

    private enum Columns
    {
        Id = 0,
        Name,
        FIGI,
        Price_Currency,
        Price_Amount,
        InstrumentId
    }

    private async Task<IReadOnlyCollection<Currency>> GetCurrencies(string sql, object? param = null)
    {
        using var reader = await connection.ExecuteReaderAsync(sql, param);

        var instruments = new List<Currency>();
        while (reader.Read())
        {
            var currency = new Currency(reader.GetInt32((int)Columns.Id));

            currency.Name = reader.GetString((int)Columns.Name);
            currency.Figi = reader.GetString((int)Columns.FIGI);
            currency.InstrumentUId = reader.GetGuid((int)Columns.InstrumentId);
            if (!reader.IsDBNull((int)Columns.Price_Currency))
            {
                currency.Price = new Money(
                    (CurrencyCode)reader.GetByte((int)Columns.Price_Currency),
                    reader.IsDBNull((int)Columns.Price_Amount) ? 0 : reader.GetDecimal((int)Columns.Price_Amount));
            }

            instruments.Add(currency);
        }

        return instruments;
    }
}