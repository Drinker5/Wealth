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

    public StrategyComponentId AddOrUpdateComponent(StockId stockId, float weight)
    {
        var c = Components.OfType<StockStrategyComponent>().FirstOrDefault(c => c.StockId == stockId);
        if (c != null)
        {
            Apply(new StrategyComponentWeightChanged(Id, c.Id, weight));
        }
        else
        {
            c = new StockStrategyComponent
            {
                Id = StrategyComponentId.New(),
                StockId = stockId,
                Weight = weight,
            };

            Apply(new StrategyComponentAdded(Id, c));
        }

        return c.Id;
    }

    public void AddOrUpdateComponent(BondId bondId, float weight)
    {
        var c = Components.OfType<BondStrategyComponent>().FirstOrDefault(c => c.BondId == bondId);
        if (c != null)
        {
            Apply(new StrategyComponentWeightChanged(Id, c.Id, c.Weight));
        }
        else
        {
            Apply(new StrategyComponentAdded(
                Id,
                new BondStrategyComponent
                {
                    Id = StrategyComponentId.New(),
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
            Apply(new StrategyComponentWeightChanged(Id, c.Id, c.Weight));
        }
        else
        {
            Apply(new StrategyComponentAdded(
                Id,
                new CurrencyStrategyComponent
                {
                    Id = StrategyComponentId.New(),
                    CurrencyId = currencyId,
                    Weight = weight,
                }));
        }
    }

    public void RemoveStrategyComponent(StockId stockId)
    {
        var component = Components.OfType<StockStrategyComponent>().SingleOrDefault(s => s.StockId == stockId);
        if (component != null)
            Apply(new StrategyComponentRemoved(Id, component.Id));
    }

    public void RemoveStrategyComponent(BondId bondId)
    {
        var component = Components.OfType<BondStrategyComponent>().SingleOrDefault(s => s.BondId == bondId);
        if (component != null)
            Apply(new StrategyComponentRemoved(Id, component.Id));
    }

    public void RemoveStrategyComponent(CurrencyId currencyId)
    {
        var component = Components.OfType<CurrencyStrategyComponent>().SingleOrDefault(s => s.CurrencyId == currencyId);
        if (component != null)
            Apply(new StrategyComponentRemoved(Id, component.Id));
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

    private void When(StrategyComponentWeightChanged @event)
    {
        var component = Components.Single(c => c.Id == @event.ComponentId);
        component.Weight = @event.Weight;
    }

    private void When(StrategyComponentAdded @event)
    {
        Components.Add(@event.Component);
    }

    private void When(StrategyComponentRemoved @event)
    {
        var component = Components.Single(s => s.Id == @event.ComponentId);
        Components.Remove(component);
    }
}