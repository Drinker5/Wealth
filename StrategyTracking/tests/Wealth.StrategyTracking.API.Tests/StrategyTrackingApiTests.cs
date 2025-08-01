using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.StrategyTracking.Application.Strategies.Queries;

namespace Wealth.StrategyTracking.API.Tests;

public sealed class StrategyTrackingApiTests : IClassFixture<StrategyTrackingApiFixture>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;

    public StrategyTrackingApiTests(StrategyTrackingApiFixture fixture)
    {
        WebApplicationFactory<Program> webApplicationFactory = fixture;
        httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task GetWallets()
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
}