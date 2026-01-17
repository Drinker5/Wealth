using AutoFixture;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SharpJuice.Essentials;
using Wealth.BuildingBlocks.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Instruments.Models;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Infrastructure.Repositories;
using Xunit;
using Assert = Xunit.Assert;

namespace Wealth.InstrumentManagement.API.Tests.RepositoriesTests;

public sealed class PricesRepositoryTests :
    IClassFixture<InstrumentManagementApiFixture>,
    IClassFixture<AutoFixtureInitializer>,
    IAsyncLifetime
{
    private readonly InstrumentManagementApiFixture apiFixture;
    private readonly PricesRepository repository;
    private readonly IFixture fixture;
    private readonly Mock<IClock> clockMock = new();
    private readonly InstrumentInitializer instrumentInitializer;
    private readonly DateTimeOffset nowTime = new(2010, 1, 2, 3, 4, 5, TimeSpan.Zero);

    public PricesRepositoryTests(InstrumentManagementApiFixture apiFixture, AutoFixtureInitializer autoFixtureInitializer)
    {
        this.apiFixture = apiFixture;
        this.fixture = autoFixtureInitializer.fixture;

        repository = new PricesRepository(
            clockMock.Object,
            apiFixture.Services.GetRequiredService<IConnectionFactory>());
        instrumentInitializer = new InstrumentInitializer(apiFixture);

        clockMock.Setup(i => i.Now).Returns(nowTime);
    }

    [Fact]
    public async Task GetOld_EmptyResult()
    {
        var result = await repository.GetOld(TimeSpan.Zero, CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task UpdatePrices_AsExpected()
    {
        var stocks = fixture.CreateMany<CreateStockCommand>(3).ToArray();
        var bonds = fixture.CreateMany<CreateBondCommand>(3).ToArray();
        var currencies = fixture.CreateMany<CreateCurrencyCommand>(3).ToArray();
        await instrumentInitializer.CreateStocks(stocks);
        await instrumentInitializer.CreateBonds(bonds);
        await instrumentInitializer.CreateCurrencies(currencies);
        var instrumentPrices = stocks.Select(i => new InstrumentUIdPrice(i.InstrumentUId, fixture.Create<decimal>()))
            .Union(bonds.Select(i => new InstrumentUIdPrice(i.InstrumentUId, fixture.Create<decimal>())))
            .Union(currencies.Select(i => new InstrumentUIdPrice(i.InstrumentUId, fixture.Create<decimal>())))
            .ToArray();

        await repository.UpdatePrices(instrumentPrices, CancellationToken.None);

        var result = await repository.GetPrices(instrumentPrices.Select(i => i.InstrumentUId).ToArray(), CancellationToken.None);
        Assert.Equivalent(instrumentPrices, result);
    }

    [Fact]
    public async Task GetOld_WhenUpdatePrices_AsExpected()
    {
        var stocks = fixture.CreateMany<CreateStockCommand>(3).ToArray();
        await instrumentInitializer.CreateStocks(stocks);
        var instrumentPrices = stocks.Select(i => new InstrumentUIdPrice(i.InstrumentUId, fixture.Create<decimal>())).ToArray();

        await repository.UpdatePrices(instrumentPrices, CancellationToken.None);

        var result1 = await repository.GetOld(TimeSpan.FromHours(1), CancellationToken.None);

        Assert.Empty(result1);

        clockMock.Setup(i => i.Now).Returns(nowTime.AddMinutes(2));
        var result2 = await repository.GetOld(TimeSpan.FromMinutes(1), CancellationToken.None);

        Assert.Equivalent(instrumentPrices.Select(i => i.InstrumentUId), result2);
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await instrumentInitializer.Clear();
    }
}