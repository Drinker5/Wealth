using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Domain.Utilities;

public static class CurrencyCodeParser
{
    public static CurrencyCode Parse(string input)
    {
        return Enum.Parse<CurrencyCode>(input, ignoreCase: true);
    }
}