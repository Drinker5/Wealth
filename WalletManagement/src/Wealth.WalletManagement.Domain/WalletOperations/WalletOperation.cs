using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.WalletManagement.Domain.WalletOperations;

public class WalletOperation
{
    public Guid Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public Money Amount { get; set; }
    public WalletId WalletId { get; set; }
    public WalletOperationType OperationType { get; set; }
}

public enum WalletOperationType
{
    Insert,
    Eject
}