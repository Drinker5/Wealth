using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.WalletManagement.Application.Wallets.Queries;

public record GetWallet(WalletId WalletId) : IQuery<WalletDTO?>;