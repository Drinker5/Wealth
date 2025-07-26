using System.Text.Json;
using Wealth.CurrencyManagement.Application.Currencies.Queries;

namespace Wealth.CurrencyManagement.Application.Tests;

public class CurrencyDTOSerializationTests
{
    private readonly JsonSerializerOptions jsonSerializerOptions = new(JsonSerializerDefaults.Web);
    
    public CurrencyDTOSerializationTests()
    {
        
    }

    [Fact]
    public void DeserializeTest()
    {
        var json = """
                   {
                     "currencyId" : {
                       "code" : "FOO"
                     },
                     "name" : "Foo",
                     "symbol" : "F"
                   }
                   """;
        
        var obj = JsonSerializer.Deserialize<CurrencyDTO>(json, jsonSerializerOptions);
        
        Assert.NotNull(obj);
        Assert.Equal("FOO", obj.CurrencyId);
        Assert.Equal("Foo", obj.Name);
        Assert.Equal("F", obj.Symbol);
    }
}