using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Domain.Tests;

public static class TestHelpers
{
    public static T HasEvent<T>(this AggregateRoot root) where T : IDomainEvent
    {
        Assert.That(root.DomainEvents, Has.Exactly(1).TypeOf<T>());
        return root.DomainEvents.OfType<T>().Single();
    }
}