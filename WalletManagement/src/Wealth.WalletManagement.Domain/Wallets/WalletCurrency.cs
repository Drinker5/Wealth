using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.WalletManagement.Domain.Wallets;

public class WalletCurrency
{
    public CurrencyCode Currency { get; init; }
    public decimal Amount { get; set; }

    public override int GetHashCode()
    {
        return Currency.GetHashCode();
    }
}