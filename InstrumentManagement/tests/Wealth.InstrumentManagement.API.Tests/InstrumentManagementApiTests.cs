using System.Text.Json;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Wealth.InstrumentManagement.API.Tests;

public sealed class InstrumentManagementApiTests : IClassFixture<InstrumentManagementApiFixture>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly InstrumentsService.InstrumentsServiceClient client;
    private readonly ICurrencyService currencyService;

    public InstrumentManagementApiTests(InstrumentManagementApiFixture fixture)
    {
        WebApplicationFactory<Program> webApplicationFactory = fixture;
        var httpClient = webApplicationFactory.CreateDefaultClient();
        var channel = GrpcChannel.ForAddress(
            httpClient.BaseAddress!,
            new GrpcChannelOptions
            {
                HttpClient = httpClient,
            });

        client = new InstrumentsService.InstrumentsServiceClient(channel);
        currencyService = fixture.CurrencyService;

        A.CallTo(() => currencyService.IsCurrencyExists(CurrencyCode.Rub)).Returns(true);
    }

    [Fact]
    public async Task GetBondFromSeeding()
    {
        var instrumentId = new BondIdProto
        {
            Id = 1,
        };

        var instrument = await client.GetBondAsync(new GetBondRequest
        {
            BondId = instrumentId
        });

        Assert.Equal(1, instrument.BondId.Id);
        Assert.Equal<decimal>(12.34m, instrument.Price.Amount);
        Assert.Equal(CurrencyCode.Rub, instrument.Price.CurrencyId);
        Assert.Equal("test-bond-1", instrument.Name);
    }

    [Fact]
    public async Task GetStockFromSeeding()
    {
        var instrumentId = new StockIdProto
        {
            Id = 1,
        };

        var instrument = await client.GetStockAsync(new GetStockRequest
        {
            StockId = instrumentId
        });

        Assert.Equal(1, instrument.StockId.Id);
        Assert.Equal(10, instrument.LotSize);
        Assert.Equal<decimal>(111m, instrument.Price.Amount);
        Assert.Equal(CurrencyCode.Rub, instrument.Price.CurrencyId);
        Assert.Equal("test-stock-1", instrument.Name);
        Assert.Equal<decimal>(222m, instrument.DividendPerYear.Amount);
        Assert.Equal(CurrencyCode.Usd, instrument.DividendPerYear.CurrencyId);
    }

    [Fact]
    public async Task WhenCreateStock()
    {
        var createStockRequest = new CreateStockRequest
        {
            Name = "Test",
            Isin = "100000000001",
            Figi = "F00000000001",
        };

        var createStockResponse = await client.CreateStockAsync(createStockRequest);

        Assert.NotEqual(0, createStockResponse.StockId.Id);
        var stockId = createStockResponse.StockId;

        var instrument = await client.GetStockAsync(new GetStockRequest { StockId = stockId });

        Assert.Equal(createStockRequest.Name, instrument.Name);
        Assert.Equal(createStockRequest.Isin, instrument.Isin);
        Assert.Equal(createStockRequest.Figi, instrument.Figi);
        Assert.Equal(0, instrument.Price.Amount);
        var newPrice = new Money(CurrencyCode.Rub, 123);

        await client.ChangeStockPriceAsync(new ChangeStockPriceRequest { StockId = stockId, Price = newPrice });

        instrument = await client.GetStockAsync(new GetStockRequest { StockId = stockId });

        Assert.Equal(newPrice, (Money)instrument.Price);
    }

    [Fact]
    public async Task WhenCreateBond()
    {
        var createStockRequest = new CreateBondRequest
        {
            Name = "Test",
            Isin = "100000000002",
            Figi = "F00000000002",
        };

        var createBondResponse = await client.CreateBondAsync(createStockRequest);

        Assert.NotEqual(0, createBondResponse.BondId.Id);
        var bondId = createBondResponse.BondId;

        var instrument = await client.GetBondAsync(new GetBondRequest
        {
            BondId = bondId
        });

        Assert.Equal(createStockRequest.Name, instrument.Name);
        Assert.Equal(createStockRequest.Isin, instrument.Isin);
        Assert.Equal(createStockRequest.Figi, instrument.Figi);
        Assert.Equal(0, instrument.Price.Amount);
        var newPrice = new Money(CurrencyCode.Rub, 123);

        await client.ChangeBondPriceAsync(new ChangeBondPriceRequest { BondId = bondId, Price = newPrice });

        instrument = await client.GetBondAsync(new GetBondRequest { BondId = bondId });

        Assert.Equal(newPrice, (Money)instrument.Price);
    }
}