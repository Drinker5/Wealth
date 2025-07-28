using Wealth.WalletManagement.Domain.WalletOperations;

namespace Wealth.WalletManagement.Domain.Repositories;

public interface IWalletOperationRepository
{
    Task AddOperation(WalletOperation walletOperation);
}