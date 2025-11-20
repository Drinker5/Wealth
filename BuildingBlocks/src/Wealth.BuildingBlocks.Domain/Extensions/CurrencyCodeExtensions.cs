using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Domain.Extensions;

public static class CurrencyCodeExtensions
{
    public static CurrencyCode FromString(string currencyString)
        => Enum.Parse<CurrencyCode>(currencyString);
}