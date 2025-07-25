using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.DepositManagement.Application.Deposits.Commands;

public record DepositWithdraw(DepositId DepositId, Money Withdrawal) : ICommand;