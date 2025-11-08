using JetBrains.Annotations;
using NSubstitute;
using SharpJuice.Clickhouse;
using Wealth.Aggregation.Infrastructure.Repositories;

namespace Wealth.Aggregation.Infrastructure.Tests.Repositories;

[TestSubject(typeof(StockTradeRepository))]
public class StockTradeRepositoryTests : IClassFixture<Fixture>
{
    private readonly Fixture fixture;
    private readonly StockTradeRepository repository;

    public StockTradeRepositoryTests(Fixture fixture)
    {
        this.fixture = fixture;
        repository = new StockTradeRepository(Substitute.For<ITableWriterBuilder>());
    }

    [Fact]
    public void METHOD()
    {
        throw new NotImplementedException();
    }
}