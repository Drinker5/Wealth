using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks;

public static class CurrencyCodeConverters
{
    public static CurrencyCode FromProto(this CurrencyCodeProto grpcValue)
    {
        return (CurrencyCode)grpcValue;
    }

    public static CurrencyCodeProto ToProto(this CurrencyCode value)
    {
        return (CurrencyCodeProto)value;
    }
}