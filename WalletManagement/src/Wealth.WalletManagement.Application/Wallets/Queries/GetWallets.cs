using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Queries;

public record GetWallets : IQuery<IReadOnlyCollection<Wallet>>;