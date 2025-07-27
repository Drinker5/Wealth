using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.WalletManagement.Domain.Wallets;

public class WalletCurrency
{
    public CurrencyId CurrencyId { get; init; }
    public decimal Amount { get; set; }

    public override int GetHashCode()
    {
        return CurrencyId.GetHashCode();
    }
}