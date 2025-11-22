using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.Deposits;
using Wealth.DepositManagement.Infrastructure.Repositories;
using Wealth.DepositManagement.Infrastructure.UnitOfWorks;

namespace Wealth.DepositManagement.Infrastructure.Tests.Repositories;

[TestSubject(typeof(DepositRepository))]
public class DepositRepositoryTests
{
    private readonly DepositRepository repository;
    private readonly WealthDbContext context;
    private const string name = "Foo";
    private readonly Yield yield = 1m;
    private const CurrencyCode currency = CurrencyCode.Rub;

    public DepositRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<WealthDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        context = new WealthDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        repository = new DepositRepository(context);
    }

    [Fact]
    public async Task WhenGetDeposits()
    {
        var result = await repository.GetDeposits();

        Assert.Empty(result);
    }

    [Fact]
    public async Task WhenGetNotExistedDeposit()
    {
        var result = await repository.GetDeposit(4);

        Assert.Null(result);
    }

    [Fact]
    public async Task WhenDepositCreated()
    {
        var id = await repository.Create(name, yield, currency);

        Assert.NotEqual(0, id.Id);

        var result = await repository.GetDeposit(id);

        Assert.NotNull(result);
        Assert.Equal(id, result.Id);
        Assert.Equal(name, result.Name);
        Assert.Equal(yield, result.Yield);
        Assert.Equal(currency, result.Investment.Currency);
    }

    [Fact]
    public async Task WhenInvest()
    {
        var id = await repository.Create(name, yield, currency);
        var investment = new Money(currency, 100m);

        await repository.Invest(id, investment);

        var deposit = await repository.GetDeposit(id);
        Assert.NotNull(deposit);
        Assert.Equal(100m, deposit.Investment.Amount);
    }

    [Fact]
    public async Task WhenWithdraw()
    {
        var id = await repository.Create(name, yield, currency);
        var investment = new Money(currency, 100m);
        var withdrawal = new Money(currency, 90m);
        await repository.Invest(id, investment);

        await repository.Withdraw(id, withdrawal);

        var deposit = await repository.GetDeposit(id);
        Assert.NotNull(deposit);
        Assert.Equal(10m, deposit.Investment.Amount);
    }
}