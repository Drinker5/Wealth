using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

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
        // get all currencies, after seeding we have 2 currencies
        var responseGet = await httpClient.GetAsync("/api/strategy/");

        responseGet.EnsureSuccessStatusCode();
    }
}