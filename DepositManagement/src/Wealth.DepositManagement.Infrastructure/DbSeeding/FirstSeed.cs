using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.BuildingBlocks.Infrastructure.EFCore.DbSeeding;
using Wealth.DepositManagement.Domain.DepositOperations;
using Wealth.DepositManagement.Domain.Deposits;
using Wealth.DepositManagement.Infrastructure.UnitOfWorks;

namespace Wealth.DepositManagement.Infrastructure.DbSeeding;

public class FirstSeed : IDbSeeder<WealthDbContext>
{
    public async Task SeedAsync(WealthDbContext context)
    {
        if (!context.Deposits.Any())
            context.Deposits.AddRange(GetPredefinedDeposits());

        if (!context.DepositOperations.Any())
            context.DepositOperations.AddRange(GetPredefinedDepositOperations());
        
        await context.SaveChangesAsync();
    }

    private static IEnumerable<Deposit> GetPredefinedDeposits()
    {
        var foo = Deposit.Create(1, "Foo", 0.20m, "RUB");
        foo.Invest(new Money("RUB", 1000));
        yield return foo;
        
        var bar = Deposit.Create(2, "Bar", 0.05m, "USD");
        bar.Invest(new Money("USD", 500));
        bar.Withdraw(new Money("USD", 100));
        
        yield return bar;
    }

    private static IEnumerable<DepositOperation> GetPredefinedDepositOperations()
    {
        yield return new DepositOperation
        {
            Id = Guid.NewGuid(),
            DepositId = 1,
            Date = Clock.Now,
            Type = DepositOperationType.Investment,
            Money = new Money("RUB", 1000),
        };
        
        yield return new DepositOperation
        {
            Id = Guid.NewGuid(),
            DepositId = 2,
            Date = Clock.Now,
            Type = DepositOperationType.Investment,
            Money = new Money("USD", 500),
        };
        
        yield return new DepositOperation
        {
            Id = Guid.NewGuid(),
            DepositId = 2,
            Date = Clock.Now,
            Type = DepositOperationType.Withdrawal,
            Money = new Money("USD", 100),
        };
    }
}