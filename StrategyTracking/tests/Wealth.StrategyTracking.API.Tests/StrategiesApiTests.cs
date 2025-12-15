using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AutoFixture;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.StrategyTracking.Application.Strategies.Queries;
using Wealth.StrategyTracking.Domain.Strategies;

namespace Wealth.StrategyTracking.API.Tests;

public sealed class StrategiesApiTests : IClassFixture<StrategyTrackingApiFixture>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;
    private readonly Fixture fixture = new Fixture();

    public StrategiesApiTests(StrategyTrackingApiFixture apiFixture)
    {
        WebApplicationFactory<Program> webApplicationFactory = apiFixture;
        httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task GetStrategies_AsExpected()
    {
        // get all strategies, after seeding we have 2
        var responseGet = await httpClient.GetAsync("/api/strategies/");

        responseGet.EnsureSuccessStatusCode();
        var currenciesJson = await responseGet.Content.ReadAsStringAsync();
        var strategies = JsonSerializer.Deserialize<IReadOnlyCollection<StrategyDTO>>(currenciesJson, jsonSerializerOptions);

        Assert.NotNull(strategies);
        var seededStrategies = strategies.Where(s => s.Name.StartsWith("Seed-strategy")).ToArray();
        Assert.Equal(2, seededStrategies.Length);
        Assert.All(seededStrategies, s => Assert.NotEmpty(s.Components));
    }

    [Fact]
    public async Task GetStrategy_AsExpected()
    {
        var responseGet = await httpClient.GetAsync("/api/strategy/1");

        responseGet.EnsureSuccessStatusCode();
        var currenciesJson = await responseGet.Content.ReadAsStringAsync();
        var strategy = JsonSerializer.Deserialize<StrategyDTO>(currenciesJson, jsonSerializerOptions);

        Assert.NotNull(strategy);
        Assert.Equal(1, strategy.StrategyId.Value);
        Assert.Collection(strategy.Components,
            c1 =>
            {
                Assert.Equal(50, c1.Weight);
                Assert.IsType<StockStrategyComponent>(c1);
            },
            c2 =>
            {
                Assert.Equal(30, c2.Weight);
                Assert.IsType<BondStrategyComponent>(c2);
            },
            c3 =>
            {
                Assert.Equal(12, c3.Weight);
                Assert.IsType<CurrencyAssetStrategyComponent>(c3);
            },
            c4 =>
            {
                Assert.Equal(8, c4.Weight);
                Assert.IsType<CurrencyStrategyComponent>(c4);
            });
    }

    [Fact]
    public async Task ChangeMasterStrategy_AsExpected()
    {
        var obj = new
        {
            name = "Test",
        };

        var createResponse = await httpClient.PostAsync("/api/strategy", JsonContent.Create(obj, options: jsonSerializerOptions));
        createResponse.EnsureSuccessStatusCode();
        var strategyId = int.Parse(await createResponse.Content.ReadAsStringAsync());
        var obj2 = new
        {
            strategyId = strategyId,
            masterStrategy = (byte)MasterStrategy.IMOEX
        };

        var followResponse = await httpClient.PostAsync("/api/strategy/follow", JsonContent.Create(obj2, options: jsonSerializerOptions));
        
        followResponse.EnsureSuccessStatusCode();
        var responseGet = await httpClient.GetAsync($"/api/strategy/{strategyId}");
        var strategyJson = await responseGet.Content.ReadAsStringAsync();
        var strategy = JsonSerializer.Deserialize<StrategyDTO>(strategyJson, jsonSerializerOptions);
        Assert.NotNull(strategy);
        Assert.Equal(MasterStrategy.IMOEX, strategy.FollowedStrategy);
    }
    
    [Fact]
    public async Task GetStrategy_NotFound()
    {
        var responseGet = await httpClient.GetAsync("/api/strategy/123");

        Assert.Equal(HttpStatusCode.NotFound, responseGet.StatusCode);
    }

    [Fact]
    public async Task CreateStrategy_AsExpected()
    {
        var obj = new
        {
            name = "Test",
        };

        var createResponse = await httpClient.PostAsync("/api/strategy", JsonContent.Create(obj, options: jsonSerializerOptions));

        createResponse.EnsureSuccessStatusCode();
        var strategyId = int.Parse(await createResponse.Content.ReadAsStringAsync());
        Assert.NotEqual(0, strategyId);

        // add instruments
        var addComponent = new
        {
            strategyId = strategyId,
            stockId = 3,
            weight = fixture.Create<float>(),
        };

        var insertResponse = await httpClient.PutAsync("/api/strategy/add-stock-component", JsonContent.Create(addComponent, options: jsonSerializerOptions));

        insertResponse.EnsureSuccessStatusCode();
        
        // get new strategy
        var responseGet = await httpClient.GetAsync($"/api/strategy/{strategyId}");

        createResponse.EnsureSuccessStatusCode();
        var strategyJson = await responseGet.Content.ReadAsStringAsync();
        var strategy = JsonSerializer.Deserialize<StrategyDTO>(strategyJson, jsonSerializerOptions);
        Assert.NotNull(strategy);
        Assert.Equal(strategyId, strategy.StrategyId.Value);
        Assert.Equal(obj.name, strategy.Name);
        var component = Assert.Single(strategy.Components);
        var stockComponent = Assert.IsType<StockStrategyComponent>(component);
        Assert.Equal(addComponent.stockId, stockComponent.StockId.Value);
        Assert.Equal(addComponent.weight, stockComponent.Weight);
    }

    [Fact]
    public async Task CreateStrategy_BadRequest()
    {
        var obj = new
        {
            name = string.Empty,
        };

        var createResponse = await httpClient.PostAsync("/api/strategy", JsonContent.Create(obj, options: jsonSerializerOptions));

        Assert.Equal(HttpStatusCode.BadRequest, createResponse.StatusCode);
    }
    
    [Fact]
    public async Task UpdateMasterStrategy_AsExpected()
    {
        var obj = new
        {
            name = "Test",
        };

        var createResponse = await httpClient.PostAsync("/api/strategy", JsonContent.Create(obj, options: jsonSerializerOptions));
        createResponse.EnsureSuccessStatusCode();
        var strategyId = int.Parse(await createResponse.Content.ReadAsStringAsync());
        var obj2 = new
        {
            strategyId = strategyId,
            masterStrategy = (byte)MasterStrategy.IMOEX
        };

        var followResponse = await httpClient.PostAsync("/api/strategy/follow", JsonContent.Create(obj2, options: jsonSerializerOptions));
        followResponse.EnsureSuccessStatusCode();
        
        var updateResponse = await httpClient.PostAsync("/api/strategy/update", JsonContent.Create(strategyId, options: jsonSerializerOptions));
        updateResponse.EnsureSuccessStatusCode();
        
        var responseGet = await httpClient.GetAsync($"/api/strategy/{strategyId}");
        var strategyJson = await responseGet.Content.ReadAsStringAsync();
        var strategy = JsonSerializer.Deserialize<StrategyDTO>(strategyJson, jsonSerializerOptions);
        Assert.NotNull(strategy);
        Assert.Equivalent(TestMoexComponentsProvider.Components, strategy.Components);
    }
}