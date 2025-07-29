using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Queries;

public record GetWallet(WalletId WalletId) : IQuery<WalletDTO?>;