using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.Deposits;
using Wealth.DepositManagement.Domain.Repositories;
using Wealth.DepositManagement.Infrastructure.UnitOfWorks;

namespace Wealth.DepositManagement.Infrastructure.Repositories;

public class DepositRepository(WealthDbContext context) : IDepositRepository
{
    public async Task<DepositId> Create(string name, Yield yield, CurrencyCode currency)
    {
        var deposit = Deposit.Create(name, yield, currency);
        await context.Deposits.AddAsync(deposit);
        return deposit.Id;
    }

    public async Task<IEnumerable<Deposit>> GetDeposits()
    {
        return await context.Deposits.AsNoTracking().ToListAsync();
    }

    public async Task<Deposit?> GetDeposit(DepositId id)
    {
        return await context.Deposits.FindAsync(id);
    }

    public async Task Invest(DepositId depositId, Money investment)
    {
        var deposit = await GetDeposit(depositId);

        deposit?.Invest(investment);
    }

    public async Task Withdraw(DepositId depositId, Money withdrawal)
    {
        var deposit = await GetDeposit(depositId);

        deposit?.Withdraw(withdrawal);
    }
}