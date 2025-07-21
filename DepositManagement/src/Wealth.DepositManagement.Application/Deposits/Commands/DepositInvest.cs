using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.DepositManagement.Application.Deposits.Commands;

public record DepositInvest(DepositId DepositId, Money Investment) : ICommand;