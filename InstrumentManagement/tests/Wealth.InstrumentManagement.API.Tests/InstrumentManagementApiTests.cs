using System.Text.Json;
using AutoFixture;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.BuildingBlocks;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Xunit;
using Assert = Xunit.Assert;

namespace Wealth.InstrumentManagement.API.Tests;

public sealed class InstrumentManagementApiTests :
    IClassFixture<InstrumentManagementApiFixture>,
    IClassFixture<AutoFixtureInitializer>
{
    private readonly IFixture fixture;
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly InstrumentsService.InstrumentsServiceClient grpcClient;

    public InstrumentManagementApiTests(InstrumentManagementApiFixture apiFixture, AutoFixtureInitializer fixtureInitializer)
    {
        fixture = fixtureInitializer.fixture;
        WebApplicationFactory<Program> webApplicationFactory = apiFixture;
        var httpClient = webApplicationFactory.CreateDefaultClient();
        var channel = GrpcChannel.ForAddress(
            httpClient.BaseAddress!,
            new GrpcChannelOptions
            {
                HttpClient = httpClient,
            });

        grpcClient = new InstrumentsService.InstrumentsServiceClient(channel);
    }

    [Xunit.Theory]
    [MemberData(nameof(GetBondRequests))]
    public async Task GetBondFromSeeding(GetBondRequest request)
    {
        var instrument = await grpcClient.GetBondAsync(request);

        Assert.Equal(1, instrument.BondId.Id);
        Assert.Equal<decimal>(12.34m, instrument.Price.Amount);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000001"), instrument.InstrumentUid);
        Assert.Equal(CurrencyCodeProto.Rub, instrument.Price.Currency);
        Assert.Equal("test-bond-1", instrument.Name);
    }

    public static TheoryData<GetBondRequest> GetBondRequests =>
    [
        new GetBondRequest { BondId = new BondIdProto { Id = 1 } },
        new GetBondRequest { Figi = "000000000001" },
        new GetBondRequest { Isin = "000000000001" },
        new GetBondRequest { InstrumentUid = "00000000-0000-0000-0000-000000000001" }
    ];

    [Xunit.Theory]
    [MemberData(nameof(GetStockRequest))]
    public async Task GetStockFromSeeding(GetStockRequest request)
    {
        var instrument = await grpcClient.GetStockAsync(request);

        Assert.Equal(1, instrument.StockInfo.StockId.Id);
        Assert.Equal(10, instrument.StockInfo.LotSize);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000003"), instrument.StockInfo.InstrumentUid);
        Assert.Equal<decimal>(111m, instrument.StockInfo.Price.Amount);
        Assert.Equal(CurrencyCodeProto.Rub, instrument.StockInfo.Price.Currency);
        Assert.Equal("test-stock-1", instrument.StockInfo.Name);
        Assert.Equal<decimal>(222m, instrument.StockInfo.DividendPerYear.Amount);
        Assert.Equal(CurrencyCodeProto.Usd, instrument.StockInfo.DividendPerYear.Currency);
    }

    public static TheoryData<GetStockRequest> GetStockRequest =>
    [
        new GetStockRequest { StockId = new StockIdProto { Id = 1 } },
        new GetStockRequest { Figi = "000000000003" },
        new GetStockRequest { Isin = "000000000003" },
        new GetStockRequest { InstrumentUid = "00000000-0000-0000-0000-000000000003" }
    ];

    [Xunit.Theory]
    [MemberData(nameof(GetCurrencyRequests))]
    public async Task GetCurrencyFromSeeding(GetCurrencyRequest request)
    {
        var instrument = await grpcClient.GetCurrencyAsync(request);

        Assert.Equal(1, instrument.CurrencyId.Id);
        Assert.Equal<decimal>(123m, instrument.Price.Amount);
        Assert.Equal(new Guid("00000000-0000-0000-0000-000000000006"), instrument.InstrumentUid);
        Assert.Equal(CurrencyCodeProto.Rub, instrument.Price.Currency);
        Assert.Equal("test-currency-1", instrument.Name);
    }

    public static TheoryData<GetCurrencyRequest> GetCurrencyRequests =>
    [
        new GetCurrencyRequest { CurrencyId = new CurrencyIdProto { Id = 1 } },
        new GetCurrencyRequest { Figi = "000000000006" },
        new GetCurrencyRequest { InstrumentUid = "00000000-0000-0000-0000-000000000006" }
    ];

    [Fact]
    public async Task WhenCreateStock()
    {
        var createStockRequest = new CreateStockRequest
        {
            Name = "Test",
            Isin = "100000000001",
            Figi = "F00000000001",
            Ticker = "FOO",
            InstrumentUid = "32026DAF-31FF-4000-A8DC-68A0E1C609F2"
        };

        var createStockResponse = await grpcClient.CreateStockAsync(createStockRequest);

        Assert.NotEqual(0, createStockResponse.StockId.Id);
        var stockId = createStockResponse.StockId;

        var instrument = await grpcClient.GetStockAsync(new GetStockRequest { StockId = stockId });

        Assert.Equal(createStockRequest.Name, instrument.StockInfo.Name);
        Assert.Equal(createStockRequest.Isin, instrument.StockInfo.Isin);
        Assert.Equal(createStockRequest.Figi, instrument.StockInfo.Figi);
        Assert.Equal(createStockRequest.Ticker, instrument.StockInfo.Ticker);
        Assert.Equal(createStockRequest.InstrumentUid, instrument.StockInfo.InstrumentUid);
        Assert.Equal(0, instrument.StockInfo.Price.Amount);
        var newPrice = new Money(CurrencyCode.Rub, 123);

        await grpcClient.ChangeStockPriceAsync(new ChangeStockPriceRequest { StockId = stockId, Price = newPrice });

        instrument = await grpcClient.GetStockAsync(new GetStockRequest { StockId = stockId });

        Assert.Equal(newPrice, (Money)instrument.StockInfo.Price);
    }

    [Fact]
    public async Task WhenCreateBond()
    {
        var createBondRequest = new CreateBondRequest
        {
            Name = "Test",
            Isin = "100000000002",
            Figi = "F00000000002",
            InstrumentUid = "F2752167-FDE5-4036-9799-F3F8C1F5454B",
        };

        var createBondResponse = await grpcClient.CreateBondAsync(createBondRequest);

        Assert.NotEqual(0, createBondResponse.BondId.Id);
        var bondId = createBondResponse.BondId;

        var instrument = await grpcClient.GetBondAsync(new GetBondRequest
        {
            BondId = bondId
        });

        Assert.Equal(createBondRequest.Name, instrument.Name);
        Assert.Equal(createBondRequest.Isin, instrument.Isin);
        Assert.Equal(createBondRequest.Figi, instrument.Figi);
        Assert.Equal(createBondRequest.InstrumentUid, instrument.InstrumentUid);
        Assert.Equal(0, instrument.Price.Amount);
        var newPrice = new Money(CurrencyCode.Rub, 123);

        await grpcClient.ChangeBondPriceAsync(new ChangeBondPriceRequest { BondId = bondId, Price = newPrice });

        instrument = await grpcClient.GetBondAsync(new GetBondRequest { BondId = bondId });

        Assert.Equal(newPrice, (Money)instrument.Price);
    }

    [Fact]
    public async Task WhenCreateCurrency()
    {
        var createCurrencyRequest = new CreateCurrencyRequest
        {
            Name = "Test",
            Figi = "F00000000003",
            InstrumentUid = "4DFB4FD6-7918-46B6-85ED-994154925001"
        };

        var createCurrencyResponse = await grpcClient.CreateCurrencyAsync(createCurrencyRequest);

        Assert.NotEqual(0, createCurrencyResponse.CurrencyId.Id);
        var currencyId = createCurrencyResponse.CurrencyId;

        var instrument = await grpcClient.GetCurrencyAsync(new GetCurrencyRequest
        {
            CurrencyId = currencyId
        });

        Assert.Equal(createCurrencyRequest.Name, instrument.Name);
        Assert.Equal(createCurrencyRequest.Figi, instrument.Figi);
        Assert.Equal(createCurrencyRequest.InstrumentUid, instrument.InstrumentUid);
        Assert.Equal(0, instrument.Price.Amount);
        var newPrice = new Money(CurrencyCode.Rub, 123);

        await grpcClient.ChangeCurrencyPriceAsync(new ChangeCurrencyPriceRequest { CurrencyId = currencyId, Price = newPrice });

        instrument = await grpcClient.GetCurrencyAsync(new GetCurrencyRequest { CurrencyId = currencyId });

        Assert.Equal(newPrice, (Money)instrument.Price);
    }

    [Fact]
    public async Task WhenGetInstruments()
    {
        var createStockRequest = new CreateStockRequest
        {
            Name = "Test",
            Isin = "200000000001",
            Figi = "F20000000001",
            Ticker = "FOO",
            InstrumentUid = "F8475E09-A5BA-4119-BACD-83862FED00AC"
        };
        var createBondRequest = new CreateBondRequest
        {
            Name = "Test",
            Isin = "200000000002",
            Figi = "F20000000002",
            InstrumentUid = "6CAE7A9F-F466-4C4A-A15E-6D1E7AC5CF4A"
        };

        await grpcClient.CreateStockAsync(createStockRequest);
        await grpcClient.CreateBondAsync(createBondRequest);
        var req = new GetInstrumentsRequest
        {
            InstrumentUids = { createStockRequest.InstrumentUid, createBondRequest.InstrumentUid }
        };

        // Act
        var response = await grpcClient.GetInstrumentsAsync(req);

        Assert.Collection(response.Instruments,
            s =>
            {
                Assert.Equal(createStockRequest.InstrumentUid, s.InstrumentUid);
                Assert.Equal(InstrumentTypeProto.Stock, s.Type);
            },
            b =>
            {
                Assert.Equal(createBondRequest.InstrumentUid, b.InstrumentUid);
                Assert.Equal(InstrumentTypeProto.Bond, b.Type);
            });
    }

    [Fact]
    public async Task ImportInstruments_ShouldCreateNewInstruments()
    {
        var instrumentIds = fixture.CreateMany<InstrumentUId>(3).ToArray();
        var createStockCommand = fixture.Create<CreateStockCommand>();
        var createBondCommand = fixture.Create<CreateBondCommand>();
        var createCurrencyCommand = fixture.Create<CreateCurrencyCommand>();
        TestTBankInstrumentsProvider.Stocks.Add(instrumentIds[0], createStockCommand);
        TestTBankInstrumentsProvider.Bonds.Add(instrumentIds[1], createBondCommand);
        TestTBankInstrumentsProvider.Currencies.Add(instrumentIds[2], createCurrencyCommand);

        var response = await grpcClient.ImportInstrumentsAsync(new ImportInstrumentsRequest
        {
            StockInstrumentIds = { instrumentIds[0] },
            BondInstrumentIds = { instrumentIds[1] },
            CurrencyInstrumentIds = { instrumentIds[2] }
        });

        Assert.Collection(response.Instruments,
            i =>
            {
                Assert.Equal(instrumentIds[0], (InstrumentUId)i.InstrumentUid);
                Assert.Equal(InstrumentTypeProto.Stock, i.Type);
            },
            i =>
            {
                Assert.Equal(instrumentIds[1], (InstrumentUId)i.InstrumentUid);
                Assert.Equal(InstrumentTypeProto.Bond, i.Type);
            },
            i =>
            {
                Assert.Equal(instrumentIds[2], (InstrumentUId)i.InstrumentUid);
                Assert.Equal(InstrumentTypeProto.CurrencyAsset, i.Type);
            });
    }

    [Fact]
    public async Task ImportInstruments_Stock_ShouldUpdateExisted()
    {
        var createStockRequest = new CreateStockRequest
        {
            InstrumentUid = fixture.Create<InstrumentUId>(),
            Isin = fixture.Create<ISIN>(),
            Figi = fixture.Create<FIGI>(),
            LotSize = fixture.Create<int>(),
            Name = fixture.Create<string>(),
            Ticker = fixture.Create<Ticker>()
        };
        var createStockResponse = await grpcClient.CreateStockAsync(createStockRequest);
        var instrumentId = fixture.Create<InstrumentUId>();
        var updateCommand = fixture.Create<CreateStockCommand>() with
        {
            Isin = createStockRequest.Isin
        };
        TestTBankInstrumentsProvider.Stocks.Add(instrumentId, updateCommand);

        // Act
        var response = await grpcClient.ImportInstrumentsAsync(new ImportInstrumentsRequest
        {
            StockInstrumentIds = { instrumentId }
        });

        var instrument = Assert.Single(response.Instruments);
        var stock = await grpcClient.GetStockAsync(new GetStockRequest { StockId = new StockIdProto(instrument.Id) });
        Assert.Equal(createStockResponse.StockId, stock.StockInfo.StockId);
        Assert.Equal(updateCommand.Figi, stock.StockInfo.Figi);
        Assert.Equal(updateCommand.Isin, stock.StockInfo.Isin.Value);
        Assert.Equal(updateCommand.LotSize.Value, stock.StockInfo.LotSize);
        Assert.Equal(updateCommand.InstrumentUId.Value, stock.StockInfo.InstrumentUid);
        Assert.Equal(updateCommand.Name, stock.StockInfo.Name);
        Assert.Equal(updateCommand.Ticker, stock.StockInfo.Ticker);
    }

    [Fact]
    public async Task ImportInstruments_Bond_ShouldUpdateExisted()
    {
        var createBondRequest = new CreateBondRequest
        {
            InstrumentUid = fixture.Create<InstrumentUId>(),
            Isin = fixture.Create<ISIN>(),
            Figi = fixture.Create<FIGI>(),
            Name = fixture.Create<string>(),
        };
        var createBondResponse = await grpcClient.CreateBondAsync(createBondRequest);
        var instrumentId = fixture.Create<InstrumentUId>();
        var updateCommand = fixture.Create<CreateBondCommand>() with
        {
            Isin = createBondRequest.Isin
        };
        TestTBankInstrumentsProvider.Bonds.Add(instrumentId, updateCommand);

        // Act
        var response = await grpcClient.ImportInstrumentsAsync(new ImportInstrumentsRequest
        {
            BondInstrumentIds = { instrumentId }
        });

        var instrument = Assert.Single(response.Instruments);
        var bond = await grpcClient.GetBondAsync(new GetBondRequest { BondId = new BondIdProto(instrument.Id) });
        Assert.Equal(createBondResponse.BondId, bond.BondId);
        Assert.Equal(updateCommand.Figi, bond.Figi);
        Assert.Equal(updateCommand.Isin, bond.Isin.Value);
        Assert.Equal(updateCommand.InstrumentUId.Value, bond.InstrumentUid);
        Assert.Equal(updateCommand.Name, bond.Name);
    }

    [Fact]
    public async Task ImportInstruments_Currency_ShouldUpdateExisted()
    {
        var createCurrencyRequest = new CreateCurrencyRequest
        {
            InstrumentUid = fixture.Create<InstrumentUId>(),
            Figi = fixture.Create<FIGI>(),
            Name = fixture.Create<string>(),
        };
        var createCurrencyResponse = await grpcClient.CreateCurrencyAsync(createCurrencyRequest);
        var instrumentUId = fixture.Create<InstrumentUId>();
        var updateCommand = fixture.Create<CreateCurrencyCommand>() with
        {
            Figi = createCurrencyRequest.Figi
        };
        TestTBankInstrumentsProvider.Currencies.Add(instrumentUId, updateCommand);

        // Act
        var response = await grpcClient.ImportInstrumentsAsync(new ImportInstrumentsRequest
        {
            CurrencyInstrumentIds = { instrumentUId }
        });

        var instrument = Assert.Single(response.Instruments);
        var stock = await grpcClient.GetCurrencyAsync(new GetCurrencyRequest { CurrencyId = new CurrencyIdProto(instrument.Id) });
        Assert.Equal(createCurrencyResponse.CurrencyId, stock.CurrencyId);
        Assert.Equal(updateCommand.Figi, stock.Figi);
        Assert.Equal(updateCommand.InstrumentUId.Value, stock.InstrumentUid);
        Assert.Equal(updateCommand.Name, stock.Name);
    }
}