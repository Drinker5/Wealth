using Wealth.BuildingBlocks.Application;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Wallets.Commands;

public record CreateWallet(string Name) : ICommand<WalletId>;