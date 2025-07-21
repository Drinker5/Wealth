using Wealth.BuildingBlocks.Domain;

namespace Wealth.DepositManagement.Domain.Tests.TestHelpers;

public static class Helpers
{
    public static T HasEvent<T>(this AggregateRoot root) where T : DomainEvent
    {
        Assert.NotNull(root.DomainEvents);
        var ev = Assert.Single(root.DomainEvents.OfType<T>());
        var concrete = Assert.IsType<T>(ev);
        return concrete;
    }
}