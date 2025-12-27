using Wealth.BuildingBlocks.Domain;
using Xunit;

namespace Wealth.BuildingBlocks.Tests.TestHelpers;

public static class Helpers
{
    extension(AggregateRoot root)
    {
        public T HasEvent<T>() where T : DomainEvent
        {
            Assert.NotNull(root.DomainEvents);
            var ev = Assert.Single(root.DomainEvents.OfType<T>());
            var concrete = Assert.IsType<T>(ev);
            return concrete;
        }

        public void HasNoEvents<T>() where T : DomainEvent
        {
            Assert.NotNull(root.DomainEvents);
            Assert.Empty(root.DomainEvents.OfType<T>());
        }
    }
}