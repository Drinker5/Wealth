using System.Data;
using Dapper;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;
using Wealth.InstrumentManagement.Infrastructure.UnitOfWorks;

namespace Wealth.InstrumentManagement.Infrastructure.Repositories;

public class BondsRepository : IBondsRepository
{
    private readonly WealthDbContext dbContext;
    private readonly IDbConnection connection;

    public BondsRepository(WealthDbContext dbContext)
    {
        this.dbContext = dbContext;
        connection = dbContext.CreateConnection();
    }

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
                           SET "Price_CurrencyId" = @CurrencyId, "Price_Amount" = @Amount
                           WHERE "Id" = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            CurrencyId = bond.Price.CurrencyId.Value,
            Amount = bond.Price.Amount,
        });
        dbContext.AddEvents(bond);
    }

    public async Task<BondId> CreateBond(string name, ISIN isin, CancellationToken token = default)
    {
        const string sql = """SELECT nextval('"BondsHiLo"')""";
        var command = new CommandDefinition(
            commandText: sql,
            cancellationToken: token);

        var nextId = await connection.ExecuteScalarAsync<int>(command);

        var bondInstrument = Bond.Create(new BondId(nextId), name, isin);
        return await CreateBond(bondInstrument);
    }

    public Task<IReadOnlyCollection<Bond>> GetBonds()
    {
        const string sql = """SELECT * FROM "Bonds" LIMIT 10""";
        return GetBonds(sql);
    }

    private async Task<BondId> CreateBond(Bond bond)
    {
        const string sql = """
                           INSERT INTO "Bonds" ("Id", "Name", "ISIN") 
                           VALUES (@Id, @Name, @ISIN)
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = bond.Id.Value,
            Name = bond.Name,
            ISIN = bond.ISIN.Value,
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
                           SET "Coupon_CurrencyId" = @CurrencyId, "Coupon_Amount" = @Amount
                           WHERE "Id" = @Id
                           """;
        await connection.ExecuteAsync(sql, new
        {
            Id = id.Value,
            CurrencyId = coupon.ValuePerYear.CurrencyId.Value,
            Amount = coupon.ValuePerYear.Amount,
        });
        dbContext.AddEvents(instrument);
    }

    private enum Columns
    {
        Id = 0,
        Name,
        ISIN,
        Price_CurrencyId,
        Price_Amount,
        Coupon_CurrencyId,
        Coupon_Amount,
    }

    private async Task<IReadOnlyCollection<Bond>> GetBonds(string sql, object? param = null)
    {
        using var reader = await connection.ExecuteReaderAsync(sql, param);

        var instruments = new List<Bond>();
        while (reader.Read())
        {
            var bond = new Bond(reader.GetInt32((int)Columns.Id));
            if (!reader.IsDBNull((int)Columns.Coupon_CurrencyId))
            {
                bond.Coupon = new Coupon(
                    reader.GetString((int)Columns.Coupon_CurrencyId),
                    reader.GetDecimal((int)Columns.Coupon_Amount));
            }

            bond.Name = reader.GetString((int)Columns.Name);
            bond.ISIN = reader.GetString((int)Columns.ISIN);
            if (!reader.IsDBNull((int)Columns.Price_CurrencyId))
            {
                bond.Price = new Money(
                    reader.GetString((int)Columns.Price_CurrencyId),
                    reader.GetDecimal((int)Columns.Price_Amount));
            }

            instruments.Add(bond);
        }

        return instruments;
    }
}