﻿using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.StrategyTracking.Application.Strategies.Queries;

namespace Wealth.StrategyTracking.API.Tests;

public sealed class StrategiesApiTests : IClassFixture<StrategyTrackingApiFixture>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;

    public StrategiesApiTests(StrategyTrackingApiFixture fixture)
    {
        WebApplicationFactory<Program> webApplicationFactory = fixture;
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
        Assert.Equal(2, strategies.Count());
        Assert.All(strategies, s => Assert.NotEmpty(s.Components));
    }

    [Fact]
    public async Task GetStrategy_AsExpected()
    {
        var responseGet = await httpClient.GetAsync("/api/strategy/1");

        responseGet.EnsureSuccessStatusCode();
        var currenciesJson = await responseGet.Content.ReadAsStringAsync();
        var strategy = JsonSerializer.Deserialize<StrategyDTO>(currenciesJson, jsonSerializerOptions);

        Assert.NotNull(strategy);
        Assert.Equal(1, strategy.StrategyId.Id);
        Assert.NotEmpty(strategy.Components);
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

        // get new strategy
        var responseGet = await httpClient.GetAsync($"/api/strategy/{strategyId}");

        createResponse.EnsureSuccessStatusCode();
        var currenciesJson = await responseGet.Content.ReadAsStringAsync();
        var strategy = JsonSerializer.Deserialize<StrategyDTO>(currenciesJson, jsonSerializerOptions);
        Assert.NotNull(strategy);
        Assert.Equal(strategyId, strategy.StrategyId.Id);
        Assert.Equal(obj.name, strategy.Name);
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
}