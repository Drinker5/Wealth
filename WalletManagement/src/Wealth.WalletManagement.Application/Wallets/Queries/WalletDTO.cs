using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Queries;

public class WalletDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<WalletCurrency> Currencies { get; set; }

    public static WalletDTO ToDTO(Wallet wallet)
    {
        return new WalletDTO
        {
            Id = wallet.Id,
            Name = wallet.Name,
            Currencies = wallet.Currencies,
        };
    }
}