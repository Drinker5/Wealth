using Wealth.BuildingBlocks.Application;

namespace Wealth.WalletManagement.Application.Wallets.Queries;

public record GetWallets : IQuery<IEnumerable<WalletDTO>>;