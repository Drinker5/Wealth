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
        const string json = """
                            {
                              "currency" : 3,
                              "name" : "Foo"
                            }
                            """;

        var obj = JsonSerializer.Deserialize<CurrencyDTO>(json, jsonSerializerOptions);

        Assert.Equal(CurrencyCode.Eur, (CurrencyCode)obj.Currency);
        Assert.Equal("Foo", obj.Name);
    }
}