using Wealth.WalletManagement.Domain.Repositories;
using Wealth.WalletManagement.Domain.WalletOperations;
using Wealth.WalletManagement.Infrastructure.UnitOfWorks;

namespace Wealth.WalletManagement.Infrastructure.Repositories;

public class WalletOperationRepository(WealthDbContext context) : IWalletOperationRepository
{
    public async Task AddOperation(WalletOperation walletOperation)
    {
        await context.WalletOperations.AddAsync(walletOperation);
    }
}