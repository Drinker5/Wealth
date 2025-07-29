﻿using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Application.Wallets.Queries;
using Assert = Xunit.Assert;

namespace Wealth.WalletManagement.API.Tests;

public sealed class WalletManagementApiTests : IClassFixture<WalletManagementApiFixture>
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    private readonly HttpClient httpClient;

    public WalletManagementApiTests(WalletManagementApiFixture fixture)
    {
        WebApplicationFactory<Program> webApplicationFactory = fixture;
        httpClient = webApplicationFactory.CreateDefaultClient();
    }

    [Fact]
    public async Task GetWallets()
    {
        // get all currencies, after seeding we have 2 currencies
        var responseGet = await httpClient.GetAsync("/api/wallet/");

        responseGet.EnsureSuccessStatusCode();
        var currenciesJson = await responseGet.Content.ReadAsStringAsync();
        var wallets = JsonSerializer.Deserialize<IEnumerable<WalletDTO>>(currenciesJson, jsonSerializerOptions);

        Assert.NotNull(wallets);
        Assert.Equal(2, wallets.Count());

        // create new wallet
        var obj = new
        {
            name = "Foo",
        };
        var response1 = await httpClient.PostAsync("/api/wallet/", JsonContent.Create(obj, options: jsonSerializerOptions));

        response1.EnsureSuccessStatusCode();
        var newWalletId = int.Parse(await response1.Content.ReadAsStringAsync());

        // get created portfolio
        var response2 = await httpClient.GetAsync($"/api/wallet/{newWalletId}");

        response2.EnsureSuccessStatusCode();
        var body = await response2.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<WalletDTO>(body, jsonSerializerOptions);

        Assert.NotNull(result);
        Assert.Equal(newWalletId, result.Id);
        Assert.Equal(obj.name, result.Name);
        Assert.Empty(result.Currencies);

        // insert
        var insertMoney = new
        {
            WalletId = newWalletId,
            money = new Money("RUB", 200)
        };

        var insertResponse = await httpClient.PutAsync("/api/wallet/insert", JsonContent.Create(insertMoney, options: jsonSerializerOptions));

        insertResponse.EnsureSuccessStatusCode();

        // eject
        var ejectMoney = new
        {
            WalletId = newWalletId,
            money = new Money("RUB", 50)
        };

        var ejectResponse = await httpClient.PutAsync("/api/wallet/eject", JsonContent.Create(ejectMoney, options: jsonSerializerOptions));

        ejectResponse.EnsureSuccessStatusCode();

        // get changed wallet
        var response4 = await httpClient.GetAsync($"/api/wallet/{newWalletId}");

        response4.EnsureSuccessStatusCode();
        var body2 = await response4.Content.ReadAsStringAsync();
        var changedWallet = JsonSerializer.Deserialize<WalletDTO>(body2, jsonSerializerOptions);

        Assert.NotNull(changedWallet);
        var currency = Assert.Single(changedWallet.Currencies);
        Assert.Equal(insertMoney.money.CurrencyId, currency.CurrencyId);
        Assert.Equal(150, currency.Amount);
    }

    [Fact]
    public async Task WhenWalletNotFound()
    {
        var response0 = await httpClient.GetAsync("/api/wallet/123");

        Assert.Equal(HttpStatusCode.NotFound, response0.StatusCode);
    }
}