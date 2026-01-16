using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SharpJuice.Essentials;
using Wealth.BuildingBlocks.Infrastructure.Repositories;
using Wealth.InstrumentManagement.Infrastructure.Repositories;
using Xunit;
using Assert = Xunit.Assert;

namespace Wealth.InstrumentManagement.API.Tests.RepositoriesTests;

public sealed class PricesRepositoryTests :
    IClassFixture<InstrumentManagementApiFixture>,
    IClassFixture<AutoFixtureInitializer>
{
    private readonly InstrumentManagementApiFixture apiFixture;
    private readonly PricesRepository repository;
    private readonly IFixture fixture;
    private readonly Mock<IClock> clockMock = new();

    public PricesRepositoryTests(InstrumentManagementApiFixture apiFixture, AutoFixtureInitializer autoFixtureInitializer)
    {
        this.apiFixture = apiFixture;
        this.fixture = autoFixtureInitializer.fixture;

        repository = new PricesRepository(
            clockMock.Object,
            apiFixture.Services.GetRequiredService<IConnectionFactory>());
    }

    [Fact]
    public async Task GetOld_EmptyResult()
    {
        var result = await repository.GetOld(TimeSpan.Zero, CancellationToken.None);

        Assert.Empty(result);
    }
    
    // todo
}