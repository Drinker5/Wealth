using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Domain.Tests.Entities;

[TestSubject(typeof(CurrencyId))]
public class CurrencyIdTests
{
    [Fact]
    public void WhenCurrencyIdCreated()
    {
        var expectedCode = CurrencyCode.Rub;
        
        var currencyId = new CurrencyId(expectedCode);

        Assert.Equal(expectedCode, currencyId.Value);
        Assert.Equal("RUB", currencyId.ToString());
    }

    [Theory]
    [InlineData("A")]
    [InlineData("AA")]
    [InlineData("AAAA")]
    [InlineData("AAA ")]
    [InlineData("aaa")]
    [InlineData("AaA")]
    public void CreateCurrencyId_InvalidCode(string code)
    {
        Assert.Throws<ArgumentException>(() => new CurrencyId(code));
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CreateCurrencyId_NullException(string? code)
    {
        Assert.Throws<ArgumentNullException>(() => new CurrencyId(code!));
    }
}