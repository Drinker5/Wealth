using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.CurrencyManagement.Application.ExchangeRates.Commands;

namespace Wealth.CurrencyManagement.API.Tests;

public sealed class ExchangeRateControllerTests : IClassFixture<CurrencyManagementApiFixture>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;
    private readonly ICommandsScheduler commandScheduler;

    public ExchangeRateControllerTests(CurrencyManagementApiFixture fixture)
    {
        WebApplicationFactory<Program> webApplicationFactory = fixture;
        httpClient = webApplicationFactory.CreateDefaultClient();
        commandScheduler = fixture.CommandsScheduler;
    }

    [Fact]
    public async Task WhenExchangeCreated()
    {
        // create new exchange rate
        var obj = new
        {
            fromId = "RUB",
            toId = "USD",
            rate = 10,
            date = "2025-10-20"
        };


        var createResponse = await httpClient.PostAsync("/api/exchangeRate/", JsonContent.Create(obj, options: jsonSerializerOptions));

        createResponse.EnsureSuccessStatusCode();
        var value = 2.3m;
        var queryParams = new Dictionary<string, string>
        {
            ["fromId"] = obj.fromId,
            ["toId"] = obj.toId,
            ["date"] = obj.date,
            ["value"] = value.ToString(CultureInfo.InvariantCulture),
        };
        var encodedParams = new FormUrlEncodedContent(queryParams);
        string queryString = await encodedParams.ReadAsStringAsync();

        var exchangeResponse = await httpClient.GetAsync($"/api/exchangeRate?{queryString}");

        exchangeResponse.EnsureSuccessStatusCode();
        var body = await exchangeResponse.Content.ReadAsStringAsync();
        var money = JsonSerializer.Deserialize<Money>(body, jsonSerializerOptions);

        Assert.Equal(obj.toId, money.Currency);
        Assert.Equal(obj.rate * value, money.Amount);
    }

    [Fact]
    public async Task WhenCheckNewExchangeRates()
    {
        // create new currency
        var cur1 = new
        {
            id = new CurrencyId(CurrencyCode.Eur),
            name = "A",
            symbol = "A"
        };
        var cur2 = new
        {
            id = new CurrencyId(CurrencyCode.Rub) // only rub is supported
        };
        (await httpClient.PostAsync("/api/currency/", JsonContent.Create(cur1, options: jsonSerializerOptions))).EnsureSuccessStatusCode();
        Clock.SetCustomDate(new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero));
        var limitCommandsPerRequest = 30;

        var checkResponse = await httpClient.PostAsync($"/api/exchangeRate/CheckNewExchangeRates/{cur1.id}/{cur2.id}", new StringContent(string.Empty));

        checkResponse.EnsureSuccessStatusCode();
        await commandScheduler.Received(limitCommandsPerRequest).ScheduleAsync(
            Arg.Any<ProvideNewExchangeRateCommand>(),
            Arg.Any<DateTimeOffset>(),
            Arg.Any<CancellationToken>());
    }
}