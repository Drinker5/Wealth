using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Application.Portfolios.Queries;
using Wealth.PortfolioManagement.Application.Services;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;
using Assert = Xunit.Assert;

namespace Wealth.PortfolioManagement.API.Tests;

public sealed class PortfolioManagementApiTests : IClassFixture<PortfolioManagementApiFixture>
{
    private readonly PortfolioManagementApiFixture fixture;
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;
    private readonly Mock<IInstrumentService> instrumentServiceMock;

    public PortfolioManagementApiTests(PortfolioManagementApiFixture fixture)
    {
        this.fixture = fixture;
        WebApplicationFactory<Program> webApplicationFactory = fixture;
        httpClient = webApplicationFactory.CreateDefaultClient();

        instrumentServiceMock = fixture.InstrumentServiceMock;
    }

    [Fact]
    public async Task CheckPortfolioIdMaps()
    {
        var scope = fixture.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WealthDbContext>();
        
        var maps = await db.PortfolioIdMaps.ToListAsync();
        
        Assert.NotEmpty(maps);
    }
    
    [Fact]
    public async Task GetInstruments()
    {
        // get all currencies, after seeding we have 2 currencies
        var responseGet = await httpClient.GetAsync("/api/portfolio/");

        responseGet.EnsureSuccessStatusCode();
        var currenciesJson = await responseGet.Content.ReadAsStringAsync();
        var portfolios = JsonSerializer.Deserialize<IEnumerable<PortfolioDTO>>(currenciesJson, jsonSerializerOptions);

        Assert.NotNull(portfolios);
        Assert.Equal(2, portfolios.Count());

        // create new portfolio
        var obj = new
        {
            name = "Foo",
        };
        var response1 = await httpClient.PostAsync("/api/portfolio/", JsonContent.Create(obj, options: jsonSerializerOptions));

        response1.EnsureSuccessStatusCode();
        var newPortfolioId = int.Parse(await response1.Content.ReadAsStringAsync());

        // get created portfolio
        var response2 = await httpClient.GetAsync($"/api/portfolio/{newPortfolioId}");

        response2.EnsureSuccessStatusCode();
        var body = await response2.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PortfolioDTO>(body, jsonSerializerOptions);

        Assert.NotNull(result);
        Assert.Equal(newPortfolioId, result.Id);
        Assert.Equal(obj.name, result.Name);
        Assert.Empty(result.Stocks);
        Assert.Empty(result.Currencies);

        // deposit
        var newDeposit = new
        {
            PortfolioId = newPortfolioId,
            money = new Money("RUB", 200)
        };

        var depositResponse = await httpClient.PutAsync("/api/portfolio/deposit", JsonContent.Create(newDeposit, options: jsonSerializerOptions));

        depositResponse.EnsureSuccessStatusCode();

        // buy asset
        var buyAsset = new
        {
            InstrumentId = new StockId(32),
            TotalPrice = new Money("RUB", 50),
            Quantity = 3,
        };

        instrumentServiceMock.Setup(i => i.GetStockInfo(buyAsset.InstrumentId)).ReturnsAsync(new StockInstrumentInfo
        {
            Id = buyAsset.InstrumentId,
            DividendPerYear = Money.Empty,
            LotSize = 1,
            Name = "Yes",
            Price = Money.Empty,
        });

        var buyAssetResponse = await httpClient.PutAsync(
            $"/api/portfolio/{newPortfolioId}/stock",
            JsonContent.Create(buyAsset, options: jsonSerializerOptions));

        buyAssetResponse.EnsureSuccessStatusCode();

        // get changed portfolio
        var response4 = await httpClient.GetAsync($"/api/portfolio/{newPortfolioId}");

        response4.EnsureSuccessStatusCode();
        var body2 = await response4.Content.ReadAsStringAsync();
        var changedPortfolio = JsonSerializer.Deserialize<PortfolioDTO>(body2, jsonSerializerOptions);

        Assert.NotNull(changedPortfolio);
        var currency = Assert.Single(changedPortfolio.Currencies);
        Assert.Equal(newDeposit.money.CurrencyId, currency.CurrencyId);
        Assert.Equal(150, currency.Amount);
        var asset = Assert.Single(changedPortfolio.Stocks);
        Assert.Equal(buyAsset.InstrumentId, asset.Id);
        Assert.Equal(3, asset.Quantity);
    }

    [Fact]
    public async Task WhenPortfolioNotFound()
    {
        var response0 = await httpClient.GetAsync("/api/portfolio/123");

        Assert.Equal(HttpStatusCode.NotFound, response0.StatusCode);
    }
}