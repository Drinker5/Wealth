using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.StrategyTracking.Domain.Strategies.Events;

namespace Wealth.StrategyTracking.Domain.Strategies;

public class Strategy : AggregateRoot
{
    public StrategyId Id { get; private set; }
    public string Name { get; private set; }

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

    public void AddOrUpdateComponent(StockId stockId, float weight)
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

    public void AddOrUpdateComponent(BondId bondId, float weight)
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

    public void AddOrUpdateComponent(CurrencyId currencyId, float weight)
    {
        var c = Components.OfType<CurrencyStrategyComponent>().FirstOrDefault(c => c.CurrencyId == currencyId);
        if (c != null)
        {
            Apply(new CurrencyStrategyComponentWeightChanged(Id, currencyId, weight));
        }
        else
        {
            Apply(new StrategyComponentAdded(
                Id,
                new CurrencyStrategyComponent
                {
                    CurrencyId = currencyId,
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
        var component = Components.OfType<CurrencyStrategyComponent>().SingleOrDefault(s => s.CurrencyId == currencyId);
        if (component != null)
            Apply(new CurrencyStrategyComponentRemoved(Id, currencyId));
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


    private void When(CurrencyStrategyComponentWeightChanged @event)
    {
        var component = Components.OfType<CurrencyStrategyComponent>().Single(c => c.CurrencyId == @event.CurrencyId);
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

    private void When(CurrencyStrategyComponentRemoved @event)
    {
        var component = Components.OfType<CurrencyStrategyComponent>().Single(s => s.CurrencyId == @event.CurrencyId);
        Components.Remove(component);
    }
}