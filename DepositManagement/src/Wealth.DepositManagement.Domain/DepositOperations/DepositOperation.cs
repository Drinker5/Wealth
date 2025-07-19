using Wealth.BuildingBlocks.Domain;
using Wealth.DepositManagement.Domain.Deposits;

namespace Wealth.DepositManagement.Domain.DepositOperations;

public class DepositOperation : IEntity
{
    public Guid Id { get; set; }
    public DepositId DepositId { get; set; }
    public DepositOperationType Type { get; set; }
    public DateTimeOffset Date { get; set; }
}