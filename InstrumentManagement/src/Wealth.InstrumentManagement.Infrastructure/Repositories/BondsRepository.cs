using System.Data;
using Dapper;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class BondsRepository(WealthDbContext dbContext) : IBondsRepository
{
    private readonly IDbConnection connection = dbContext.CreateConnection();

    public async Task<Bond?> GetBond(BondId id)
    {
        const string sql = """SELECT * FROM "Bonds" WHERE "Id" = @Id""";
        var instruments = await GetBonds(sql, new { Id = id.Value });
        return instruments.FirstOrDefault();
    }

    public async Task<Bond?> GetBond(ISIN isin)
    {
        const string sql = """SELECT * FROM "Bonds" WHERE "ISIN" = @isin""";
        var instruments = await GetBonds(sql, new { isin = isin.Value });
        return instruments.FirstOrDefault();
    }

    public async Task<Bond?> GetBond(FIGI figi)
    {
        const string sql = """SELECT * FROM "Bonds" WHERE "FIGI" = @figi""";
        var instruments = await GetBonds(sql, new { figi = figi.Value });
        return instruments.FirstOrDefault();
    }

    public async Task<Bond?> GetBond(InstrumentId id)
    {
        const string sql = """SELECT * FROM "Bonds" WHERE instrument_id = @instrumentId""";
        var instruments = await GetBonds(sql, new { instrumentId = id.Value });
        return instruments.FirstOrDefault();
    }

    public async Task DeleteBond(BondId instrumentId)
    {
        const string sql = """DELETE FROM "Bonds" WHERE "Id" = @Id""";
        await connection.ExecuteAsync(sql, new { Id = instrumentId.Value });
    }

    public async Task ChangePrice(BondId id, Money price)
    {
        var bond = await GetBond(id);
        if (bond == null)
            return;

        bond.ChangePrice(price);
        const string sql = """
                           UPDATE "Bonds" 
                           SET "Price_Currency" = @Currency, "Price_Amount" = @Amount
                           WHERE "Id" = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            Currency = bond.Price.Currency,
            Amount = bond.Price.Amount,
        });
        dbContext.AddEvents(bond);
    }

    public async Task<BondId> CreateBond(CreateBondCommand command, CancellationToken token = default)
    {
        const string sql = """SELECT nextval('"BondsHiLo"')""";

        var nextId = await connection.ExecuteScalarAsync<int>(new CommandDefinition(
            commandText: sql,
            cancellationToken: token));

        var bondInstrument = Bond.Create(new BondId(nextId), command.Name, command.Isin, command.Figi, command.InstrumentId);
        return await CreateBond(bondInstrument);
    }

    public async Task<BondId> UpsertBond(CreateBondCommand command, CancellationToken token)
    {
        var bond = await GetBond(command.Isin, command.Figi, command.InstrumentId, token);
        if (bond == null)
            return await CreateBond(command, token);

        await UpdateBond(bond, command, token);
        return bond.Id;
    }

    private async Task UpdateBond(Bond instrument, CreateBondCommand command, CancellationToken token)
    {
        instrument.ChangeName(command.Name);
        instrument.ChangeIsin(command.Isin);
        instrument.ChangeFigi(command.Figi);
        instrument.ChangeInstrumentId(command.InstrumentId);
        await connection.ExecuteAsync(
            """
            UPDATE "Bonds" 
            SET "Name" = @Name, "ISIN" = @Isin, "FIGI" = @Figi, instrument_id = @InstrumentId
            WHERE "Id" = @Id
            """,
            new
            {
                Id = instrument.Id.Value,
                Name = command.Name,
                ISIN = command.Isin.Value,
                Figi = command.Figi.Value,
                InstrumentId = command.InstrumentId.Value
            });
        dbContext.AddEvents(instrument);
    }

    public Task<IReadOnlyCollection<Bond>> GetBonds()
    {
        const string sql = """SELECT * FROM "Bonds" LIMIT 10""";
        return GetBonds(sql);
    }

    private async Task<Bond?> GetBond(ISIN isin, FIGI figi, InstrumentId instrumentId, CancellationToken token)
    {
        var instruments = await GetBonds(
            """
            SELECT * FROM "Bonds"
            WHERE "ISIN" = @Isin
                OR "FIGI" = @Figi
                OR instrument_id = @InstrumentId
            """,
            new
            {
                Isin = isin.Value,
                Figi = figi.Value,
                InstrumentId = instrumentId.Value
            });

        if (instruments.Count > 1)
            throw new InvalidOperationException($"Found more than one bond Isin: ${isin.Value}, Figi: ${figi.Value}, InstrumentId: ${instrumentId.Value}");

        return instruments.FirstOrDefault();
    }

    private async Task<BondId> CreateBond(Bond bond)
    {
        const string sql = """
                           INSERT INTO "Bonds" ("Id", "Name", "ISIN", "FIGI", instrument_id) 
                           VALUES (@Id, @Name, @ISIN, @FIGI, @InstrumentId)
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = bond.Id.Value,
            Name = bond.Name,
            ISIN = bond.Isin.Value,
            FIGI = bond.Figi.Value,
            InstrumentId = bond.InstrumentId.Value
        });
        dbContext.AddEvents(bond);

        return bond.Id;
    }

    public async Task ChangeCoupon(BondId id, Coupon coupon)
    {
        var instrument = await GetBond(id);
        if (instrument == null)
            return;

        instrument.ChangeCoupon(coupon);
        const string sql = """
                           UPDATE "Bonds" 
                           SET "Coupon_Currency" = @Currency, "Coupon_Amount" = @Amount
                           WHERE "Id" = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            Currency = coupon.ValuePerYear.Currency,
            Amount = coupon.ValuePerYear.Amount,
        });
        dbContext.AddEvents(instrument);
    }

    private enum Columns
    {
        Id = 0,
        Name,
        ISIN,
        Price_Amount,
        Coupon_Amount,
        FIGI,
        Price_Currency,
        Coupon_Currency,
        InstrumentId
    }

    private async Task<IReadOnlyCollection<Bond>> GetBonds(string sql, object? param = null)
    {
        using var reader = await connection.ExecuteReaderAsync(sql, param);

        var instruments = new List<Bond>();
        while (reader.Read())
        {
            var bond = new Bond(reader.GetInt32((int)Columns.Id));
            if (!reader.IsDBNull((int)Columns.Coupon_Currency))
            {
                bond.Coupon = new Coupon(
                    (CurrencyCode)reader.GetByte((int)Columns.Coupon_Currency),
                    reader.GetDecimal((int)Columns.Coupon_Amount));
            }

            bond.Name = reader.GetString((int)Columns.Name);
            bond.Isin = reader.GetString((int)Columns.ISIN);
            bond.Figi = reader.GetString((int)Columns.FIGI);
            bond.InstrumentId = reader.GetGuid((int)Columns.InstrumentId);
            if (!reader.IsDBNull((int)Columns.Price_Currency))
            {
                bond.Price = new Money(
                    (CurrencyCode)reader.GetByte((int)Columns.Price_Currency),
                    reader.GetDecimal((int)Columns.Price_Amount));
            }

            instruments.Add(bond);
        }

        return instruments;
    }
}