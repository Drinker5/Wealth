using System.Text.Json;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Application.Currencies.Queries;

namespace Wealth.CurrencyManagement.Application.Tests;

public class CurrencyDTOSerializationTests
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);

    [Fact]
    public void DeserializeTest()
    {
        var json = """
                   {
                     "currencyId" : 3,
                     "name" : "Foo",
                     "symbol" : "F"
                   }
                   """;

        var obj = JsonSerializer.Deserialize<CurrencyDTO>(json, jsonSerializerOptions);

        Assert.NotNull(obj);
        Assert.Equal(CurrencyCode.Eur, obj.CurrencyId.Value);
        Assert.Equal("Foo", obj.Name);
        Assert.Equal("F", obj.Symbol);
    }
}