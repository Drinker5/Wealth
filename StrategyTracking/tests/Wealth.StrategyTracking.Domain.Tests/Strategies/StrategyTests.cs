using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Tests.TestHelpers;
using Wealth.StrategyTracking.Domain.Strategies;
using Wealth.StrategyTracking.Domain.Strategies.Events;

namespace Wealth.StrategyTracking.Domain.Tests.Strategies;

[TestSubject(typeof(Strategy))]
public class StrategyTests
{
    private readonly Strategy strategy;
    private readonly string name = "Foo";
    private readonly StockId stockId1 = new StockId(3);

    public StrategyTests()
    {
        strategy = Strategy.Create(name);
    }

    [Fact]
    public void Create_AsExpected()
    {
        Assert.Equal(0, strategy.Id.Value);
        Assert.Equal(name, strategy.Name);
        Assert.Empty(strategy.Components);
        var ev = strategy.HasEvent<StrategyCreated>();
        Assert.Same(strategy, ev.Strategy);
    }

    [Fact]
    public void Create_EmptyName()
    {
        Assert.ThrowsAny<ArgumentException>(() => Strategy.Create(string.Empty));
    }

    [Fact]
    public void Rename()
    {
        var newName = "bar";

        strategy.Rename(newName);

        Assert.Equal(newName, strategy.Name);
        var ev = strategy.HasEvent<StrategyRenamed>();
        Assert.Equal(strategy.Id, ev.StrategyId);
        Assert.Equal(newName, ev.NewName);
    }

    [Fact]
    public void AddComponent()
    {
        var weight = 0.42f;

        strategy.AddOrUpdateComponent(stockId1, weight);

        var component = Assert.Single(strategy.Components.OfType<StockStrategyComponent>());
        Assert.Equal(stockId1, component.StockId);
        Assert.Equal(weight, component.Weight);
        var ev = strategy.HasEvent<StrategyComponentAdded>();
        Assert.Equal(strategy.Id, ev.StrategyId);
        var stockComponent = Assert.IsType<StockStrategyComponent>(ev.Component);
        Assert.Same(component, stockComponent);
    }

    [Fact]
    public void RemoveComponent_AsExpected()
    {
        var componentId = strategy.AddOrUpdateComponent(stockId1, 0.42f);

        strategy.RemoveStrategyComponent(stockId1);

        Assert.Empty(strategy.Components);
        var ev = strategy.HasEvent<StrategyComponentRemoved>();
        Assert.Equal(strategy.Id, ev.StrategyId);
        
        Assert.Equal(componentId, ev.ComponentId);
    }

    [Fact]
    public void RemoveComponent_NotExisted()
    {
        strategy.RemoveStrategyComponent(stockId1);

        strategy.HasNoEvents<StrategyComponentRemoved>();
    }
}