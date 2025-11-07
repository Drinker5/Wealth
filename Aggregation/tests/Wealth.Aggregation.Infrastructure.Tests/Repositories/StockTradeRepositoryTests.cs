using JetBrains.Annotations;
using NSubstitute;
using SharpJuice.Clickhouse;
using Wealth.Aggregation.Infrastructure.Repositories;

namespace Wealth.Aggregation.Infrastructure.Tests.Repositories;

[TestSubject(typeof(StockTradeRepository))]
public class StockTradeRepositoryTests
{
    private readonly StockTradeRepository repository;

    public StockTradeRepositoryTests()
    {
        repository = new StockTradeRepository(Substitute.For<ITableWriterBuilder>());
    }

    [Fact]
    public void METHOD()
    {
        throw new NotImplementedException();
    }
}