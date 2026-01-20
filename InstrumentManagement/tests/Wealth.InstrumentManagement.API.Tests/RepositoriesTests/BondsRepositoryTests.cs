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

public sealed class BondsRepositoryTests :
    IClassFixture<InstrumentManagementApiFixture>,
    IClassFixture<AutoFixtureInitializer>,
    IAsyncLifetime
{
    private readonly InstrumentManagementApiFixture apiFixture;
    private readonly BondsRepository repository;
    private readonly IFixture fixture;
    private readonly Mock<IClock> clockMock = new();
    private readonly InstrumentInitializer instrumentInitializer;
    private readonly DateTimeOffset nowTime = new(2010, 1, 2, 3, 4, 5, TimeSpan.Zero);

    public BondsRepositoryTests(InstrumentManagementApiFixture apiFixture, AutoFixtureInitializer autoFixtureInitializer)
    {
        this.apiFixture = apiFixture;
        this.fixture = autoFixtureInitializer.fixture;

        repository = new BondsRepository(
            apiFixture.Services.GetRequiredService<IConnectionFactory>(),
            apiFixture.Services.GetRequiredService<IEventTracker>(),
            clockMock.Object);
        instrumentInitializer = new InstrumentInitializer(apiFixture);

        clockMock.Setup(i => i.Now).Returns(nowTime);
    }

    [Fact]
    public async Task GetBonds_EmptyResult()
    {
        var result = await repository.GetBonds([], CancellationToken.None);

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetBonds_AsExpected()
    {
        var bonds = fixture.CreateMany<CreateBondCommand>(3).ToDictionary(i => i.InstrumentUId);
        await instrumentInitializer.CreateBonds(bonds.Values);

        var result = await repository.GetBonds(bonds.Keys, CancellationToken.None);

        Assert.Equal(bonds.Count, result.Count);
        foreach (var id in bonds.Keys)
        {
            var actual = result[id];
            var expected = bonds[id];
            
            Assert.Equal(expected.Currency, actual.Price.Currency);
            Assert.Equal(expected.Figi, actual.Figi);
            Assert.Equal(expected.Isin, actual.Isin);
            Assert.Equal(expected.Name, actual.Name);
        }
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public async Task DisposeAsync()
    {
        await instrumentInitializer.Clear();
    }
}