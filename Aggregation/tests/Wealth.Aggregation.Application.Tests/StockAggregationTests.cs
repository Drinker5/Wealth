using AutoFixture;
using JetBrains.Annotations;
using Wealth.Aggregation.Application.Models;

namespace Wealth.Aggregation.Application.Tests;

[TestSubject(typeof(StockAggregation))]
public class StockAggregationTests
{
    private readonly StockAggregation aggregation;

    public StockAggregationTests()
    {
        var fixture = new Fixture();
        aggregation = fixture.Create<StockAggregation>();
    }
}