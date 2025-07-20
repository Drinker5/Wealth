using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.Deposits;

namespace Wealth.DepositManagement.Domain.Repositories;

public interface IDepositRepository
{
    Task<DepositId> Create(string name, Yield yield, CurrencyId currency);
    Task<IEnumerable<Deposit>> GetDeposits();
    Task<Deposit?> GetDeposit(DepositId id);
    Task Invest(DepositId depositId, Money investment);
    Task Withdraw(DepositId depositId, Money withdrawal);
}