using Wealth.BuildingBlocks.Domain;
using Xunit;

namespace Wealth.BuildingBlocks.Tests.TestHelpers;

public static class Helpers
{
    public static T HasEvent<T>(this AggregateRoot root) where T : DomainEvent
    {
        Assert.NotNull(root.DomainEvents);
        var ev = Assert.Single(root.DomainEvents.OfType<T>());
        var concrete = Assert.IsType<T>(ev);
        return concrete;
    }

    public static void HasNoEvents<T>(this AggregateRoot root) where T : DomainEvent
    {
        Assert.NotNull(root.DomainEvents);
        Assert.Empty(root.DomainEvents.OfType<T>());
    }
}