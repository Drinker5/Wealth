using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.Deposits;
using Wealth.DepositManagement.Domain.Deposits.Events;
using Wealth.DepositManagement.Domain.Tests.TestHelpers;

namespace Wealth.DepositManagement.Domain.Tests.Deposits;

[TestSubject(typeof(Deposit))]
public class DepositTests
{
    private readonly string name = "foo";
    private Yield yield = new Yield(23.42m);
    private CurrencyId currencyId = "RUB";
    private readonly Deposit deposit;

    public DepositTests()
    {
        deposit = Deposit.Create(name, yield, currencyId);
    }

    [Fact]
    public void WhenCreate()
    {
        Assert.Equal(0, deposit.Id.Id);
        Assert.Equal(name, deposit.Name);
        Assert.Equal(yield, deposit.Yield);
        Assert.Equal(0, deposit.Investment.Amount);
        Assert.Equal(currencyId, deposit.Investment.CurrencyId);
        Assert.Equal(0, deposit.InterestPerYear.Amount);
        Assert.Equal(currencyId, deposit.InterestPerYear.CurrencyId);
    }

    [Fact]
    public void WhenChangeName()
    {
        var newName = "new name";

        deposit.ChangeName(newName);

        Assert.Equal(newName, deposit.Name);
        var ev = deposit.HasEvent<DepositRenamed>();
        Assert.Equal(newName, ev.Name);
    }

    [Fact]
    public void WhenChangeYield()
    {
        var newYield = new Yield(3.2m);

        deposit.ChangeYield(newYield);

        Assert.Equal(newYield, deposit.Yield);
        var ev = deposit.HasEvent<DepositYieldChanged>();
        Assert.Equal(newYield, ev.Yield);
    }

    [Fact]
    public void WhenInvest()
    {
        var investment = new Money(currencyId, 10);

        deposit.Invest(investment);

        Assert.Equal(investment, deposit.Investment);
        var ev = deposit.HasEvent<DepositInvested>();
        Assert.Equal(investment, ev.Investment);
    }

    [Fact]
    public void WhenInvestInterestChanged()
    {
        var investment = new Money(currencyId, 10);

        deposit.Invest(investment);
        deposit.Invest(investment);

        Assert.Equal(deposit.InterestPerYear, 2 * investment * deposit.Yield);
    }

    [Fact]
    public void WhenInvestDifferentCurrency()
    {
        var investment = new Money("USD", 10);

        Assert.Throws<InvalidOperationException>(() => deposit.Invest(investment));
    }

    [Fact]
    public void WhenWithdraw()
    {
        var investment = new Money(currencyId, 10);
        var withdraw = new Money(currencyId, 9);
        deposit.Invest(investment);
        
        deposit.Withdraw(withdraw);
        
        Assert.Equal(1, deposit.Investment.Amount);
        var ev = deposit.HasEvent<DepositWithdrew>();
        Assert.Equal(withdraw, ev.Withdraw);
    }
    
    [Fact]
    public void WhenWithdrawDifferentCurrency()
    {
        var investment = new Money("RUB", 10);
        deposit.Invest(investment);
        var withdraw = new Money("USD", 10);

        Assert.Throws<InvalidOperationException>(() => deposit.Withdraw(withdraw));
    }
    
    [Fact]
    public void WhenOverWithdrew()
    {
        var withdraw = new Money(currencyId, 10);
        
        Assert.Throws<InvalidOperationException>(() => deposit.Withdraw(withdraw));
    }
}