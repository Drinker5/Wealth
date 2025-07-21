using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.DepositManagement.Domain.Deposits.Events;

public record DepositInvested(DepositId DepositId, Money Investment) : DomainEvent;