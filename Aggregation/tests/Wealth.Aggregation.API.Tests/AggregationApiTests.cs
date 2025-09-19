using Microsoft.AspNetCore.Mvc.Testing;

namespace Wealth.Aggregation.API.Tests;

public sealed class AggregationApiTests : IClassFixture<AggregationApiFixture>
{
    private readonly AggregationApiFixture fixture;
    private readonly HttpClient httpClient;

    public AggregationApiTests(AggregationApiFixture fixture)
    {
        this.fixture = fixture;
        WebApplicationFactory<Program> webApplicationFactory = fixture;
        httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Fact]
    public void CanLaunch()
    {
    }
}