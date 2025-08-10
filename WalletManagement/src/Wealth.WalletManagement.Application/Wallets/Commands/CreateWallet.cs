using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.WalletManagement.Application.Wallets.Commands;

public record CreateWallet(string Name) : ICommand<WalletId>;