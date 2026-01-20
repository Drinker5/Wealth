using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SharpJuice.Essentials;
using Wealth.BuildingBlocks.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Infrastructure.Repositories;
using Xunit;
using Assert = Xunit.Assert;

namespace Wealth.InstrumentManagement.API.Tests.RepositoriesTests;

public sealed class CurrenciesRepositoryTests :
    IClassFixture<InstrumentManagementApiFixture>,
    IClassFixture<AutoFixtureInitializer>,
    IAsyncLifetime
{
    private readonly InstrumentManagementApiFixture apiFixture;
    private readonly CurrenciesRepository repository;
    private readonly IFixture fixture;
    private readonly Mock<IClock> clockMock = new();
    private readonly InstrumentInitializer instrumentInitializer;
    private readonly DateTimeOffset nowTime = new(2010, 1, 2, 3, 4, 5, TimeSpan.Zero);

    public CurrenciesRepositoryTests(InstrumentManagementApiFixture apiFixture, AutoFixtureInitializer autoFixtureInitializer)
    {
        this.apiFixture = apiFixture;
        this.fixture = autoFixtureInitializer.fixture;

        repository = new CurrenciesRepository(
            apiFixture.Services.GetRequiredService<IConnectionFactory>(),
            apiFixture.Services.GetRequiredService<IEventTracker>(),
            clockMock.Object);
        instrumentInitializer = new InstrumentInitializer(apiFixture);

        clockMock.Setup(i => i.Now).Returns(nowTime);
    }

    [Fact]
    public async Task GetCurrencies_EmptyResult()
    {
        var result = await repository.GetCurrencies([], CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetCurrencies_AsExpected()
    {
        var currencies = fixture.CreateMany<CreateCurrencyCommand>(3).ToDictionary(i => i.InstrumentUId);
        await instrumentInitializer.CreateCurrencies(currencies.Values);

        var result = await repository.GetCurrencies(currencies.Keys, CancellationToken.None);

        Assert.Equal(currencies.Count, result.Count);
        foreach (var id in currencies.Keys)
        {
            var actual = result[id];
            var expected = currencies[id];
            
            Assert.Equal(expected.Currency, actual.Price.Currency);
            Assert.Equal(expected.Figi, actual.Figi);
            Assert.Equal(expected.Name, actual.Name);
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await instrumentInitializer.Clear();
    }
}