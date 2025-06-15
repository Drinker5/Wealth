using Wealth.CurrencyManagement.Domain.Interfaces;

namespace Wealth.CurrencyManagement.Domain.Tests.Entities;

public static class TestHelpers
{
    public static T HasEvent<T>(this AggregateRoot root) where T : IDomainEvent
    {
        var ev = Assert.Single(root.Events.OfType<T>());
        var concrete = Assert.IsType<T>(ev);
        return concrete;
    }
}