using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies;
using Wealth.StrategyTracking.Infrastructure.Repositories;
using Wealth.StrategyTracking.Infrastructure.UnitOfWorks;

namespace Wealth.StrategyTracking.Infrastructure.Tests.Repositories;

[TestSubject(typeof(StrategyRepository))]
public class StrategyRepositoryTests
{
    private readonly StrategyRepository repository;
    private readonly WealthDbContext context;
    private readonly StockId instrumentId1 = 3;
    private readonly BondId instrumentId2 = 4;

    public StrategyRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<WealthDbContext>()
            .UseInMemoryDatabase("fakeDb")
            .Options;
        context = new WealthDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        repository = new StrategyRepository(context);
    }

    [Fact]
    public async Task Repository_GetStrategies()
    {
        var portfolios = await repository.GetStrategies();
        
        Assert.Empty(portfolios);
    }

    [Fact]
    public async Task Repository_CreateStrategy_ThenIdGenerated()
    {
        var strategy = Strategy.Create("Foo");

        await context.Strategies.AddAsync(strategy);

        Assert.NotEqual(0, strategy.Id.Id);
    }

    [Fact]
    public async Task Repository_StrategyCreated_ThenIdReturned()
    {
        var id = await CreateStrategy("Foo");
        
        Assert.NotEqual(0, id.Id);
        var portfolio = await repository.GetStrategy(id);
        Assert.NotNull(portfolio);
        Assert.Equal("Foo", portfolio.Name);
    }

    [Fact]
    public async Task Repository_AddStrategyComponent()
    {
        var weight = 23.23f;
        var id = await CreateStrategy("Foo");

        await repository.AddStrategyComponent(id, instrumentId1, weight);

        var strategy = await repository.GetStrategy(id);
        Assert.NotNull(strategy);
        var component = Assert.Single(strategy.Components.OfType<StockStrategyComponent>());
        
        Assert.Equal(instrumentId1, component.StockId);
        Assert.Equal(weight, component.Weight);
    }

    [Fact]
    public async Task Repository_RemoveStrategyComponent()
    {
        var id = await CreateStrategy("Foo");
        await repository.AddStrategyComponent(id, instrumentId1, 23.23f);

        await repository.RemoveStrategyComponent(id, instrumentId1);

        var strategy = await repository.GetStrategy(id);
        Assert.NotNull(strategy);
        Assert.Empty(strategy.Components);
    }

    [Fact]
    public async Task Repository_UpdateStrategyComponent()
    {
        var weight = 13.13f;
        var id = await CreateStrategy("Foo");
        await repository.AddStrategyComponent(id, instrumentId1, 23.23f);

        await repository.ChangeStrategyComponentWeight(id, instrumentId1, weight);

        var strategy = await repository.GetStrategy(id);
        Assert.NotNull(strategy);
        var component = Assert.Single(strategy.Components.OfType<StockStrategyComponent>());
        Assert.Equal(instrumentId1, component.StockId);
        Assert.Equal(weight, component.Weight);
    }

    private async Task<StrategyId> CreateStrategy(string strategyName)
    {
        var id = await repository.CreateStrategy(strategyName);
        await context.SaveChangesAsync();
        return id;
    }
}