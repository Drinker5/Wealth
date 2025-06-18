using Wealth.CurrencyManagement.Domain.Abstractions;

namespace Wealth.CurrencyManagement.Domain.Tests.Entities;

public static class TestHelpers
{
    public static T HasEvent<T>(this AggregateRoot root) where T : IDomainEvent
    {
        var ev = Assert.Single(root.DomainEvents.OfType<T>());
        var concrete = Assert.IsType<T>(ev);
        return concrete;
    }
}