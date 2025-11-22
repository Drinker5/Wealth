using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.CurrencyManagement.Application.Currencies.Queries;

[StructLayout(LayoutKind.Auto)]
public record struct CurrencyDTO(
    byte Currency,
    string Name)
{
    public static CurrencyDTO From(CurrencyCode currency)
    {
        return new CurrencyDTO((byte)currency, currency.ToString());
    }
}