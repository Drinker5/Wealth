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

    public void AddOrUpdateComponent(InstrumentId instrumentId, float weight)
    {
        var component = Components.SingleOrDefault(s => s.InstrumentId == instrumentId);
        if (component != null)
        {
            Apply(new StrategyComponentWeightChanged(Id, instrumentId, weight));
            component.Weight = weight;
            return;
        }

        Apply(new StrategyComponentAdded(Id, instrumentId, weight));
    }

    public void RemoveStrategyComponent(InstrumentId instrumentId)
    {
        var component = Components.SingleOrDefault(s => s.InstrumentId == instrumentId);
        if (component != null)
        {
            Apply(new StrategyComponentRemoved(Id, instrumentId));
        }
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
        var component = Components.Single(s => s.InstrumentId == @event.InstrumentId);
        component.Weight = @event.Weight;
    }

    private void When(StrategyComponentAdded @event)
    {
        Components.Add(new StrategyComponent
        {
            InstrumentId = @event.InstrumentId,
            Weight = @event.Weight
        });
    }

    private void When(StrategyComponentRemoved @event)
    {
        var component = Components.Single(s => s.InstrumentId == @event.InstrumentId);
        Components.Remove(component);
    }
}