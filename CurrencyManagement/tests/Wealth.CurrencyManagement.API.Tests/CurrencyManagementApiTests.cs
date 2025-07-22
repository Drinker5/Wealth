using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.CurrencyManagement.Application.Currencies.Queries;

namespace Wealth.CurrencyManagement.API.Tests;

public sealed class CurrencyManagementApiTests : IClassFixture<CurrencyManagementApiFixture>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;

    public CurrencyManagementApiTests(CurrencyManagementApiFixture fixture)
    {
        WebApplicationFactory<Program> webApplicationFactory = fixture;
        httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task GetCurrencies()
    {
        var response = await httpClient.GetAsync("/api/currency/");

        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<IEnumerable<CurrencyDTO>>(body, jsonSerializerOptions);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
}
