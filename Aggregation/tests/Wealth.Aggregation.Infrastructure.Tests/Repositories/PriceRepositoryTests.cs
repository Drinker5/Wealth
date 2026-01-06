using JetBrains.Annotations;
using Octonica.ClickHouseClient;
using SharpJuice.Clickhouse;
using Wealth.Aggregation.Application.Models;
using Wealth.Aggregation.Infrastructure.Repositories;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.Aggregation.Infrastructure.Tests.Repositories;

[TestSubject(typeof(PriceRepository))]
public class PriceRepositoryTests : IClassFixture<ClickHouseFixture>
{
    private readonly PriceRepository _repository;
    private readonly ClickHouseConnectionFactory _connectionFactory;

    public PriceRepositoryTests(ClickHouseFixture clickHouseFixture)
    {
        var clickHouseConnectionStringBuilder = new ClickHouseConnectionStringBuilder(clickHouseFixture.ClickHouseConnectionString);
        var clickHouseConnectionSettings = clickHouseConnectionStringBuilder.BuildSettings();
        _connectionFactory = new ClickHouseConnectionFactory(clickHouseConnectionSettings);
        _repository = new PriceRepository(new TableWriterBuilder(_connectionFactory), _connectionFactory);
    }

    [Fact]
    public async Task ChangePrices_InsertNewPrices_ShouldStoreInDatabase()
    {
        // Arrange
        var instrumentPrices = new List<InstrumentPrice>
        {
            CreateInstrumentPrice(1, InstrumentType.Stock, 100.50m),
            CreateInstrumentPrice(2, InstrumentType.Bond, 200.75m),
            CreateInstrumentPrice(3, InstrumentType.CurrencyAsset, 150.25m)
        };

        // Act
        await _repository.ChangePrices(instrumentPrices, CancellationToken.None);

        // Assert
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(CancellationToken.None);

        const string query = """
                             SELECT instrument_id, instrument_type, price 
                             FROM instrument_price_view FINAL
                             WHERE instrument_id IN (1, 2, 3)
                             ORDER BY instrument_id
                             """;

        await using var command = connection.CreateCommand(query);
        await using var reader = await command.ExecuteReaderAsync(CancellationToken.None);

        var results = new List<(int Id, byte Type, decimal Price)>();
        while (await reader.ReadAsync(CancellationToken.None))
            results.Add((reader.GetInt32(0), reader.GetByte(1), reader.GetDecimal(2)));

        Assert.Equal(3, results.Count);

        Assert.Equal(1, results[0].Id);
        Assert.Equal((byte)InstrumentType.Stock, results[0].Type);
        Assert.Equal(100.50m, results[0].Price);

        Assert.Equal(2, results[1].Id);
        Assert.Equal((byte)InstrumentType.Bond, results[1].Type);
        Assert.Equal(200.75m, results[1].Price);

        Assert.Equal(3, results[2].Id);
        Assert.Equal((byte)InstrumentType.CurrencyAsset, results[2].Type);
        Assert.Equal(150.25m, results[2].Price);
    }

    [Fact]
    public async Task ChangePrices_UpdateExistingPrices_ShouldReplaceOldValues()
    {
        // Arrange - сначала добавляем старые значения
        await InsertTestPrices();

        var updatedPrices = new List<InstrumentPrice>
        {
            CreateInstrumentPrice(1, InstrumentType.Stock, 300.99m), // Обновляем цену
            CreateInstrumentPrice(4, InstrumentType.Stock, 400.00m) // Добавляем новый
        };

        // Act
        await _repository.ChangePrices(updatedPrices, CancellationToken.None);

        // Assert
        var prices = await _repository.GetPrices(
            new[] { new InstrumentId(1), new InstrumentId(2), new InstrumentId(4) },
            CancellationToken.None);

        Assert.Equal(300.99m, prices[new InstrumentId(1)]);
        Assert.Equal(200.75m, prices[new InstrumentId(2)]); // Старое значение осталось
        Assert.Equal(400.00m, prices[new InstrumentId(4)]); // Новое значение добавилось
    }

    [Fact]
    public async Task GetPrices_WithExistingInstruments_ShouldReturnCorrectPrices()
    {
        // Arrange
        await InsertTestPrices();

        var instrumentIds = new[]
        {
            new InstrumentId(1),
            new InstrumentId(2),
            new InstrumentId(3)
        };

        // Act
        var result = await _repository.GetPrices(instrumentIds, CancellationToken.None);

        // Assert
        Assert.Equal(3, result.Count);
        Assert.Equal(100.50m, result[new InstrumentId(1)]);
        Assert.Equal(200.75m, result[new InstrumentId(2)]);
        Assert.Equal(150.25m, result[new InstrumentId(3)]);
    }

    [Fact]
    public async Task GetPrices_WithNonExistingInstruments_ShouldReturnEmptyDictionary()
    {
        // Arrange
        await InsertTestPrices();

        var nonExistingIds = new[]
        {
            new InstrumentId(999),
            new InstrumentId(1000)
        };

        // Act
        var result = await _repository.GetPrices(nonExistingIds, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPrices_WithMixedExistingAndNonExisting_ShouldReturnOnlyExisting()
    {
        // Arrange
        await InsertTestPrices();

        var mixedIds = new[]
        {
            new InstrumentId(1), // Существует
            new InstrumentId(999), // Не существует
            new InstrumentId(2) // Существует
        };

        // Act
        var result = await _repository.GetPrices(mixedIds, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(100.50m, result[new InstrumentId(1)]);
        Assert.Equal(200.75m, result[new InstrumentId(2)]);
        Assert.False(result.ContainsKey(new InstrumentId(999)));
    }

    [Fact]
    public async Task GetPrices_WithEmptyCollection_ShouldReturnEmptyDictionary()
    {
        // Arrange
        await InsertTestPrices();

        var emptyIds = Array.Empty<InstrumentId>();

        // Act
        var result = await _repository.GetPrices(emptyIds, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetPrices_UsingDictionaryView_ShouldReturnCorrectData()
    {
        // Arrange
        await InsertTestPrices();

        // Перезагружаем словари, чтобы данные точно были актуальны
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(CancellationToken.None);
        await connection.CreateCommand("SYSTEM RELOAD DICTIONARIES").ExecuteNonQueryAsync(CancellationToken.None);

        var instrumentIds = new[] { new InstrumentId(1) };

        // Act
        var result = await _repository.GetPrices(instrumentIds, CancellationToken.None);

        // Assert
        var price = Assert.Single(result);
        Assert.Equal(new InstrumentId(1), price.Key);
        Assert.Equal(100.50m, price.Value);
    }

    private async Task InsertTestPrices(CancellationToken token = default)
    {
        await using var connection = _connectionFactory.Create();
        await connection.OpenAsync(token);

        const string insertQuery =
            """
            INSERT INTO instrument_price (instrument_id, instrument_type, price) 
            VALUES (@instrumentId, @instrumentType, @price)
            """;

        var prices = new[]
        {
            (Id: 1, Type: InstrumentType.Stock, Price: 100.50m),
            (Id: 2, Type: InstrumentType.Bond, Price: 200.75m),
            (Id: 3, Type: InstrumentType.CurrencyAsset, Price: 150.25m)
        };

        foreach (var price in prices)
        {
            await using var command = connection.CreateCommand(insertQuery);
            command.Parameters.AddWithValue("@instrumentId", price.Id);
            command.Parameters.AddWithValue("@instrumentType", (byte)price.Type);
            command.Parameters.AddWithValue("@price", price.Price);

            await command.ExecuteNonQueryAsync(token);
        }
    }

    private static InstrumentPrice CreateInstrumentPrice(int instrumentId, InstrumentType type, decimal priceAmount)
    {
        var instrumentIdType = new InstrumentIdType(new InstrumentId(instrumentId), type);
        var price = new Money(CurrencyCode.Cny, priceAmount);
        return new InstrumentPrice(instrumentIdType, price);
    }
}