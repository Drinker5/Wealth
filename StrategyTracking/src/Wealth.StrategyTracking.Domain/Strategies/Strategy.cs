using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies.Events;

namespace Wealth.StrategyTracking.Domain.Strategies;

public sealed class Strategy : AggregateRoot
{
    public StrategyId Id { get; private set; }
    public string Name { get; private set; }
    public MasterStrategy FollowedStrategy { get; private set; }
    public List<StrategyComponent> Components { get; } = [];

    private Strategy()
    {
    }

    public static Strategy Create(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentException("Name cannot be null or empty", nameof(name));

        var strategy = new Strategy
        {
            Id = StrategyId.New(),
            Name = name
        };

        strategy.Apply(new StrategyCreated(strategy));
        return strategy;
    }

    public void Rename(string newName)
    {
        if (Name == newName)
            return;

        Apply(new StrategyRenamed(Id, newName));
    }

    public void Follow(MasterStrategy toFollow)
    {
        if (FollowedStrategy == toFollow)
            return;

        Apply(new MasterStrategyFollowed(Id, toFollow));
    }

    public void AddOrUpdateComponent(StockId stockId, decimal weight)
    {
        var c = Components.OfType<StockStrategyComponent>().FirstOrDefault(c => c.StockId == stockId);
        if (c != null)
        {
            Apply(new StockStrategyComponentWeightChanged(Id, stockId, weight));
        }
        else
        {
            c = new StockStrategyComponent
            {
                StockId = stockId,
                Weight = weight,
            };

            Apply(new StrategyComponentAdded(Id, c));
        }
    }

    public void AddOrUpdateComponent(BondId bondId, decimal weight)
    {
        var c = Components.OfType<BondStrategyComponent>().FirstOrDefault(c => c.BondId == bondId);
        if (c != null)
        {
            Apply(new BondStrategyComponentWeightChanged(Id, bondId, weight));
        }
        else
        {
            Apply(new StrategyComponentAdded(
                Id,
                new BondStrategyComponent
                {
                    BondId = bondId,
                    Weight = weight,
                }));
        }
    }

    public void AddOrUpdateComponent(CurrencyId currencyId, decimal weight)
    {
        var c = Components.OfType<CurrencyAssetStrategyComponent>().FirstOrDefault(c => c.CurrencyId == currencyId);
        if (c != null)
        {
            Apply(new CurrencyAssetStrategyComponentWeightChanged(Id, currencyId, weight));
        }
        else
        {
            Apply(new StrategyComponentAdded(
                Id,
                new CurrencyAssetStrategyComponent
                {
                    CurrencyId = currencyId,
                    Weight = weight,
                }));
        }
    }

    public void AddOrUpdateComponent(CurrencyCode currency, decimal weight)
    {
        var c = Components.OfType<CurrencyStrategyComponent>().FirstOrDefault(c => c.Currency == currency);
        if (c != null)
        {
            Apply(new CurrencyStrategyComponentWeightChanged(Id, currency, weight));
        }
        else
        {
            Apply(new StrategyComponentAdded(
                Id,
                new CurrencyStrategyComponent
                {
                    Currency = currency,
                    Weight = weight,
                }));
        }
    }

    public void RemoveStrategyComponent(StockId stockId)
    {
        var component = Components.OfType<StockStrategyComponent>().SingleOrDefault(s => s.StockId == stockId);
        if (component != null)
            Apply(new StockStrategyComponentRemoved(Id, stockId));
    }

    public void RemoveStrategyComponent(BondId bondId)
    {
        var component = Components.OfType<BondStrategyComponent>().SingleOrDefault(s => s.BondId == bondId);
        if (component != null)
            Apply(new BondStrategyComponentRemoved(Id, bondId));
    }

    public void RemoveStrategyComponent(CurrencyId currencyId)
    {
        var component = Components.OfType<CurrencyAssetStrategyComponent>().SingleOrDefault(s => s.CurrencyId == currencyId);
        if (component != null)
            Apply(new CurrencyAssetStrategyComponentRemoved(Id, currencyId));
    }

    public void RemoveStrategyComponent(CurrencyCode currency)
    {
        var component = Components.OfType<CurrencyStrategyComponent>().SingleOrDefault(s => s.Currency == currency);
        if (component != null)
            Apply(new CurrencyStrategyComponentRemoved(Id, currency));
    }

    public void SetComponents(IReadOnlyCollection<StrategyComponent> newComponents)
    {
        if (newComponents.Count > 0)
        {
            var totalWeight = newComponents.Sum(c => c.Weight);
            const decimal tolerance = 1m;

            if (Math.Abs(totalWeight - 100) > tolerance)
                throw new ArgumentException($"Total weight must be 100.0 (currently: {totalWeight})", nameof(newComponents));
        }

        var duplicateStocks = newComponents.OfType<StockStrategyComponent>()
            .GroupBy(c => c.StockId)
            .Count(g => g.Count() > 1);

        var duplicateBonds = newComponents.OfType<BondStrategyComponent>()
            .GroupBy(c => c.BondId)
            .Count(g => g.Count() > 1);

        var duplicateCurrencyAssets = newComponents.OfType<CurrencyAssetStrategyComponent>()
            .GroupBy(c => c.CurrencyId)
            .Count(g => g.Count() > 1);

        var duplicateCurrencies = newComponents.OfType<CurrencyStrategyComponent>()
            .GroupBy(c => c.Currency)
            .Count(g => g.Count() > 1);

        var duplicates = duplicateStocks + duplicateBonds + duplicateCurrencyAssets + duplicateCurrencies;

        if (duplicates > 0)
            throw new ArgumentException("Duplicate components found in the collection", nameof(newComponents));

        var currentStocks = Components.OfType<StockStrategyComponent>();
        var newStocks = newComponents.OfType<StockStrategyComponent>().ToDictionary(i => i.StockId);

        var currentBonds = Components.OfType<BondStrategyComponent>();
        var newBonds = newComponents.OfType<BondStrategyComponent>().ToDictionary(i => i.BondId);

        var currentCurrencyAssets = Components.OfType<CurrencyAssetStrategyComponent>();
        var newCurrencyAssets = newComponents.OfType<CurrencyAssetStrategyComponent>().ToDictionary(i => i.CurrencyId);

        var currentCurrencies = Components.OfType<CurrencyStrategyComponent>();
        var newCurrencies = newComponents.OfType<CurrencyStrategyComponent>().ToDictionary(i => i.Currency);

        var stocksToRemove = currentStocks.Where(i => !newStocks.ContainsKey(i.StockId)).ToList();
        var bondsToRemove = currentBonds.Where(i => !newBonds.ContainsKey(i.BondId)).ToList();
        var currencyAssetsToRemove = currentCurrencyAssets.Where(i => !newCurrencyAssets.ContainsKey(i.CurrencyId)).ToList();
        var currenciesToRemove = currentCurrencies.Where(i => !newCurrencies.ContainsKey(i.Currency)).ToList();

        foreach (var newStock in newStocks.Values)
            AddOrUpdateComponent(newStock.StockId, newStock.Weight);

        foreach (var newBond in newBonds.Values)
            AddOrUpdateComponent(newBond.BondId, newBond.Weight);

        foreach (var newCurrencyAsset in newCurrencyAssets.Values)
            AddOrUpdateComponent(newCurrencyAsset.CurrencyId, newCurrencyAsset.Weight);

        foreach (var newCurrency in newCurrencies.Values)
            AddOrUpdateComponent(newCurrency.Currency, newCurrency.Weight);

        foreach (var stockToRemove in stocksToRemove)
            RemoveStrategyComponent(stockToRemove.StockId);

        foreach (var bondToRemove in bondsToRemove)
            RemoveStrategyComponent(bondToRemove.BondId);

        foreach (var currencyAssetToRemove in currencyAssetsToRemove)
            RemoveStrategyComponent(currencyAssetToRemove.CurrencyId);

        foreach (var currencyToRemove in currenciesToRemove)
            RemoveStrategyComponent(currencyToRemove.Currency);
    }

    private void When(StrategyCreated @event)
    {
        Id = @event.Strategy.Id;
        Name = @event.Strategy.Name;
    }

    private void When(StrategyRenamed @event)
    {
        Name = @event.NewName;
    }

    private void When(StockStrategyComponentWeightChanged @event)
    {
        var component = Components.OfType<StockStrategyComponent>().Single(c => c.StockId == @event.StockId);
        component.Weight = @event.Weight;
    }

    private void When(BondStrategyComponentWeightChanged @event)
    {
        var component = Components.OfType<BondStrategyComponent>().Single(c => c.BondId == @event.BondId);
        component.Weight = @event.Weight;
    }

    private void When(CurrencyAssetStrategyComponentWeightChanged @event)
    {
        var component = Components.OfType<CurrencyAssetStrategyComponent>().Single(c => c.CurrencyId == @event.CurrencyId);
        component.Weight = @event.Weight;
    }

    private void When(CurrencyStrategyComponentWeightChanged @event)
    {
        var component = Components.OfType<CurrencyStrategyComponent>().Single(c => c.Currency == @event.Currency);
        component.Weight = @event.Weight;
    }

    private void When(StrategyComponentAdded @event)
    {
        Components.Add(@event.Component);
    }

    private void When(StockStrategyComponentRemoved @event)
    {
        var component = Components.OfType<StockStrategyComponent>().Single(s => s.StockId == @event.StockId);
        Components.Remove(component);
    }

    private void When(BondStrategyComponentRemoved @event)
    {
        var component = Components.OfType<BondStrategyComponent>().Single(s => s.BondId == @event.BondId);
        Components.Remove(component);
    }

    private void When(CurrencyAssetStrategyComponentRemoved @event)
    {
        var component = Components.OfType<CurrencyAssetStrategyComponent>().Single(s => s.CurrencyId == @event.CurrencyId);
        Components.Remove(component);
    }

    private void When(CurrencyStrategyComponentRemoved @event)
    {
        var component = Components.OfType<CurrencyStrategyComponent>().Single(s => s.Currency == @event.Currency);
        Components.Remove(component);
    }

    private void When(MasterStrategyFollowed @event)
    {
        FollowedStrategy = @event.NewFollowedStrategy;
    }
}