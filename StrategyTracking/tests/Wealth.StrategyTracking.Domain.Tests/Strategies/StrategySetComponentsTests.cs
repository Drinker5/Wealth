using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies;
using Wealth.StrategyTracking.Domain.Strategies.Events;

namespace Wealth.StrategyTracking.Domain.Tests.Strategies;

[TestSubject(typeof(Strategy))]
public class StrategySetComponentsTests
{
    private readonly Strategy _strategy;
    private readonly StockId _stockId1 = new StockId(1);
    private readonly StockId _stockId2 = new StockId(2);
    private readonly BondId _bondId1 = new BondId(1);
    private readonly BondId _bondId2 = new BondId(2);
    private readonly CurrencyId _currencyId1 = new CurrencyId(1);
    private readonly CurrencyId _currencyId2 = new CurrencyId(2);
    private readonly CurrencyCode _currencyCode1 = CurrencyCode.Usd;
    private readonly CurrencyCode _currencyCode2 = CurrencyCode.Eur;

    public StrategySetComponentsTests()
    {
        _strategy = Strategy.Create("Test Strategy");
    }

    [Fact]
    public void SetComponents_WithValidComponents_ShouldReplaceAllComponents()
    {
        // Arrange
        var initialComponents = new List<StrategyComponent>
        {
            new StockStrategyComponent { StockId = _stockId1, Weight = 50 },
            new StockStrategyComponent { StockId = _stockId2, Weight = 50 }
        };

        _strategy.AddOrUpdateComponent(_stockId1, 50);
        _strategy.AddOrUpdateComponent(_stockId2, 50);
        _strategy.ClearDomainEvents();

        var newComponents = new List<StrategyComponent>
        {
            new BondStrategyComponent { BondId = _bondId1, Weight = 30 },
            new CurrencyStrategyComponent { Currency = _currencyCode1, Weight = 70 }
        };

        // Act
        _strategy.SetComponents(newComponents);

        // Assert
        Assert.Equal(2, _strategy.Components.Count);
        Assert.Single(_strategy.Components.OfType<BondStrategyComponent>());
        Assert.Single(_strategy.Components.OfType<CurrencyStrategyComponent>());

        var bondComponent = _strategy.Components.OfType<BondStrategyComponent>().First();
        var currencyComponent = _strategy.Components.OfType<CurrencyStrategyComponent>().First();

        Assert.Equal(_bondId1, bondComponent.BondId);
        Assert.Equal(30, bondComponent.Weight);
        Assert.Equal(_currencyCode1, currencyComponent.Currency);
        Assert.Equal(70, currencyComponent.Weight);
    }

    [Fact]
    public void SetComponents_WithTotalWeightNotEqualToOne_ShouldThrowException()
    {
        // Arrange
        var invalidComponents = new List<StrategyComponent>
        {
            new StockStrategyComponent { StockId = _stockId1, Weight = 30 },
            new StockStrategyComponent { StockId = _stockId2, Weight = 30 }
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _strategy.SetComponents(invalidComponents));
        Assert.Contains("Total weight must be 100.0", exception.Message);
    }

    [Fact]
    public void SetComponents_WithDuplicateStockIds_ShouldThrowException()
    {
        // Arrange
        var duplicateComponents = new List<StrategyComponent>
        {
            new StockStrategyComponent { StockId = _stockId1, Weight = 50 },
            new StockStrategyComponent { StockId = _stockId1, Weight = 50 }
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _strategy.SetComponents(duplicateComponents));
        Assert.Contains("Duplicate components found", exception.Message);
    }

    [Fact]
    public void SetComponents_WithMixedDuplicateComponents_ShouldThrowException()
    {
        // Arrange
        var duplicateComponents = new List<StrategyComponent>
        {
            new StockStrategyComponent { StockId = _stockId1, Weight = 40 },
            new BondStrategyComponent { BondId = _bondId1, Weight = 30 },
            new StockStrategyComponent { StockId = _stockId1, Weight = 30 }
        };

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() =>
            _strategy.SetComponents(duplicateComponents));
        Assert.Contains("Duplicate components found", exception.Message);
    }

    [Fact]
    public void SetComponents_WithEmptyCollection_ShouldClearAllComponents()
    {
        // Arrange
        _strategy.AddOrUpdateComponent(_stockId1, 100);
        _strategy.ClearDomainEvents();

        // Act
        _strategy.SetComponents(new List<StrategyComponent>());

        // Assert
        Assert.Empty(_strategy.Components);
    }

    [Fact]
    public void SetComponents_ShouldGenerateAppropriateEvents()
    {
        // Arrange
        _strategy.AddOrUpdateComponent(_stockId1, 100);
        _strategy.ClearDomainEvents();

        var newComponents = new List<StrategyComponent>
        {
            new StockStrategyComponent { StockId = _stockId2, Weight = 60 },
            new BondStrategyComponent { BondId = _bondId1, Weight = 40 }
        };

        // Act
        _strategy.SetComponents(newComponents);

        // Assert
        Assert.NotNull(_strategy.DomainEvents);
        var addedEvents = _strategy.DomainEvents
            .Where(e => e is StrategyComponentAdded)
            .Cast<StrategyComponentAdded>()
            .ToList();

        var removedEvents = _strategy.DomainEvents
            .Where(e => e.GetType().Name.Contains("Removed"))
            .ToList();

        // del StockId1, add StockId2 BondId1
        Assert.Single(removedEvents);
        Assert.Equal(2, addedEvents.Count);

        var stockRemovedEvent = Assert.IsType<StockStrategyComponentRemoved>(removedEvents.First());
        Assert.Equal(_stockId1, stockRemovedEvent.StockId);

        Assert.Contains(addedEvents, e =>
            e.Component is StockStrategyComponent s && s.StockId == _stockId2);
        Assert.Contains(addedEvents, e =>
            e.Component is BondStrategyComponent b && b.BondId == _bondId1);
    }

    [Fact]
    public void SetComponents_WithSameComponentsDifferentWeights_ShouldUpdateWeights()
    {
        // Arrange
        _strategy.AddOrUpdateComponent(_stockId1, 50);
        _strategy.AddOrUpdateComponent(_stockId2, 50);
        _strategy.ClearDomainEvents();

        var sameComponentsNewWeights = new List<StrategyComponent>
        {
            new StockStrategyComponent { StockId = _stockId1, Weight = 70 },
            new StockStrategyComponent { StockId = _stockId2, Weight = 30 }
        };

        // Act
        _strategy.SetComponents(sameComponentsNewWeights);

        // Assert
        Assert.NotNull(_strategy.DomainEvents);
        var weightChangedEvents = _strategy.DomainEvents
            .Where(e => e.GetType().Name.Contains("WeightChanged"))
            .ToList();

        Assert.Equal(2, weightChangedEvents.Count);

        var stock1Event = Assert.IsType<StockStrategyComponentWeightChanged>(
            weightChangedEvents.First(e =>
                ((StockStrategyComponentWeightChanged)e).StockId == _stockId1));
        var stock2Event = Assert.IsType<StockStrategyComponentWeightChanged>(
            weightChangedEvents.First(e =>
                ((StockStrategyComponentWeightChanged)e).StockId == _stockId2));

        Assert.Equal(70, stock1Event.Weight);
        Assert.Equal(30, stock2Event.Weight);
    }

    [Fact]
    public void SetComponents_WithAllTypesOfComponents_ShouldWorkCorrectly()
    {
        // Arrange
        var allTypeComponents = new List<StrategyComponent>
        {
            new StockStrategyComponent { StockId = _stockId1, Weight = 25 },
            new BondStrategyComponent { BondId = _bondId1, Weight = 25 },
            new CurrencyAssetStrategyComponent { CurrencyId = _currencyId1, Weight = 25 },
            new CurrencyStrategyComponent { Currency = _currencyCode1, Weight = 25 }
        };

        // Act
        _strategy.SetComponents(allTypeComponents);

        // Assert
        Assert.Equal(4, _strategy.Components.Count);
        Assert.Single(_strategy.Components.OfType<StockStrategyComponent>());
        Assert.Single(_strategy.Components.OfType<BondStrategyComponent>());
        Assert.Single(_strategy.Components.OfType<CurrencyAssetStrategyComponent>());
        Assert.Single(_strategy.Components.OfType<CurrencyStrategyComponent>());
    }
}