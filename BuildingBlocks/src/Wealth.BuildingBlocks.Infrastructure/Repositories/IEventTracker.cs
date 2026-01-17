using Wealth.BuildingBlocks.Domain;

namespace Wealth.BuildingBlocks.Infrastructure.Repositories;

public interface IEventTracker
{
    public void AddEvents(AggregateRoot aggregate);
}