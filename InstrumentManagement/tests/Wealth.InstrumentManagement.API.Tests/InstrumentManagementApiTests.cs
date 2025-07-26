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
        
        A.CallTo(() => currencyService.IsCurrencyExists("RUB")).Returns(true);
    }

    [Fact]
    public async Task GetBond()
    {
        var instrumentId = new InstrumentIdProto
        {
            Id = new Guid("00000000-0000-0000-0000-000000000001"),
        };

        var instrument = await client.GetInstrumentAsync(new GetInstrumentRequest
        {
            Id = instrumentId
        });

        Assert.Equal(GetInstrumentResponse.InstrumentOneofCase.BondInfo, instrument.InstrumentCase);
        Assert.Equal("test-bond-1", instrument.Name);
    }

    [Fact]
    public async Task GetStock()
    {
        var instrumentId = new InstrumentIdProto
        {
            Id = new Guid("00000000-0000-0000-0000-000000000003"),
        };

        var instrument = await client.GetInstrumentAsync(new GetInstrumentRequest
        {
            Id = instrumentId
        });

        Assert.Equal(GetInstrumentResponse.InstrumentOneofCase.StockInfo, instrument.InstrumentCase);
        Assert.Equal("test-stock-1", instrument.Name);
    }

    [Fact]
    public async Task WhenCreateStock()
    {
        var createStockRequest = new CreateStockRequest
        {
            Name = "Test",
            Isin = "100000000001",
        };

        var createStockResponse = await client.CreateStockAsync(createStockRequest);

        Assert.NotEmpty(createStockResponse.Id.Id.Value);
        InstrumentId stockId = createStockResponse.Id;

        var instrument = await client.GetInstrumentAsync(new GetInstrumentRequest { Id = stockId });

        Assert.Equal(GetInstrumentResponse.InstrumentOneofCase.StockInfo, instrument.InstrumentCase);
        Assert.Equal(createStockRequest.Name, instrument.Name);
        Assert.Equal(createStockRequest.Isin, instrument.Isin);
        Assert.Equal(0, instrument.Price.Price);
        var newPrice = new Money("RUB", 123);

        await client.ChangePriceAsync(new ChangePriceRequest { Id = stockId, Price = newPrice });
        
        instrument = await client.GetInstrumentAsync(new GetInstrumentRequest { Id = stockId });
        
        Assert.Equal(newPrice, (Money)instrument.Price);
    }

    [Fact]
    public async Task WhenCreateBond()
    {
        var createStockRequest = new CreateBondRequest
        {
            Name = "Test",
            Isin = "100000000002",
        };

        var createBondResponse = await client.CreateBondAsync(createStockRequest);

        Assert.NotEmpty(createBondResponse.Id.Id.Value);
        InstrumentId bondId = createBondResponse.Id;

        var instrument = await client.GetInstrumentAsync(new GetInstrumentRequest
        {
            Id = bondId
        });
        
        Assert.Equal(GetInstrumentResponse.InstrumentOneofCase.BondInfo, instrument.InstrumentCase);
        Assert.Equal(createStockRequest.Name, instrument.Name);
        Assert.Equal(createStockRequest.Isin, instrument.Isin);
        Assert.Equal(0, instrument.Price.Price);
        var newPrice = new Money("RUB", 123);

        await client.ChangePriceAsync(new ChangePriceRequest { Id = bondId, Price = newPrice });
        
        instrument = await client.GetInstrumentAsync(new GetInstrumentRequest { Id = bondId });
        
        Assert.Equal(newPrice, (Money)instrument.Price);
    }
}