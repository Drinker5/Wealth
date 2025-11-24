using JetBrains.Annotations;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Extensions;

namespace Wealth.Aggregation.API.Tests.Extensions;

[TestSubject(typeof(CurrencyCodeExtensions))]
public class CurrencyCodeExtensionsTests
{
    [Theory]
    [InlineData("rub", CurrencyCode.Rub)]
    [InlineData("Rub", CurrencyCode.Rub)]
    [InlineData("RUB", CurrencyCode.Rub)]
    [InlineData("cny", CurrencyCode.Cny)]
    [InlineData("cnY", CurrencyCode.Cny)]
    [InlineData("eur", CurrencyCode.Eur)]
    [InlineData("eUr", CurrencyCode.Eur)]
    public void FromString_Works(string from, CurrencyCode expected)
    {
        var actual = CurrencyCodeExtensions.FromString(from);
        
        Assert.Equal(expected, actual);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("Foo")]
    public void FromString_NotExpected_Throws(string from)
    {
        Assert.Throws<ArgumentException>(() => CurrencyCodeExtensions.FromString(from));
    }
}