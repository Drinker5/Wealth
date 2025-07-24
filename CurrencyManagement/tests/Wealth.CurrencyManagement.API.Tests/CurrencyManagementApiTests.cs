using System.Net;
using System.Net.Http.Json;
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
    public async Task AddAndChangeCurrency()
    {
        // get all currencies, after seeding we have 2 currencies
        var responseGet = await httpClient.GetAsync("/api/currency/");

        responseGet.EnsureSuccessStatusCode();
        var currenciesJson = await responseGet.Content.ReadAsStringAsync();
        var currencies = JsonSerializer.Deserialize<IEnumerable<CurrencyDTO>>(currenciesJson, jsonSerializerOptions);

        Assert.NotNull(currencies);
        Assert.Equal(2, currencies.Count());

        // get not existed currency
        var currencyId = "FOO";
        var response0 = await httpClient.GetAsync($"/api/currency/{currencyId}");

        Assert.Equal(HttpStatusCode.NotFound, response0.StatusCode);

        // create new currency
        var obj = new
        {
            id = currencyId,
            name = "Foo",
            symbol = "F"
        };

        var response1 = await httpClient.PostAsync("/api/currency/", JsonContent.Create(obj, options: jsonSerializerOptions));

        response1.EnsureSuccessStatusCode();

        // get created currency
        var response2 = await httpClient.GetAsync($"/api/currency/{currencyId}");

        response2.EnsureSuccessStatusCode();
        var body = await response2.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<CurrencyDTO>(body, jsonSerializerOptions);

        Assert.NotNull(result);
        Assert.Equal(currencyId, result.CurrencyId);
        Assert.Equal(obj.name, result.Name);
        Assert.Equal(obj.symbol, result.Symbol);

        // change currency name
        var newName = new
        {
            id = currencyId,
            newName = "NewName"
        };
        
        var response3 = await httpClient.PutAsync("/api/currency/", JsonContent.Create(newName, options: jsonSerializerOptions));

        response3.EnsureSuccessStatusCode();

        // get changed currency
        var response4 = await httpClient.GetAsync($"/api/currency/{currencyId}");

        response4.EnsureSuccessStatusCode();
        var body2 = await response4.Content.ReadAsStringAsync();
        var result2 = JsonSerializer.Deserialize<CurrencyDTO>(body2, jsonSerializerOptions);

        Assert.NotNull(result2);
        Assert.Equal(currencyId, result2.CurrencyId);
        Assert.Equal(newName.newName, result2.Name);
        Assert.Equal(obj.symbol, result2.Symbol);
        
        // get all currencies
        var responseGet2 = await httpClient.GetAsync("/api/currency/");

        responseGet2.EnsureSuccessStatusCode();
        var currenciesJson2 = await responseGet2.Content.ReadAsStringAsync();
        var currencies2 = JsonSerializer.Deserialize<IEnumerable<CurrencyDTO>>(currenciesJson2, jsonSerializerOptions);

        Assert.NotNull(currencies2);
        Assert.Equal(3, currencies2.Count());
    }

    [Fact]
    public async Task WhenInvalidCurrency()
    {
        var response0 = await httpClient.GetAsync("/api/currency/QWERTY");

        Assert.Equal(HttpStatusCode.BadRequest, response0.StatusCode);
    }
}