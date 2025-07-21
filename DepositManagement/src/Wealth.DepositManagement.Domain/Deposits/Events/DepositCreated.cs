using Wealth.BuildingBlocks.Domain;

namespace Wealth.DepositManagement.Domain.Deposits.Events;

public record DepositCreated(Deposit Deposit) : DomainEvent;