using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.DepositManagement.Domain.Deposits.Events;

public record DepositWithdrew(DepositId DepositId, Money Withdraw) : DomainEvent;