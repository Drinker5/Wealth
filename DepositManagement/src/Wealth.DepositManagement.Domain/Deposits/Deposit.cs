using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.DepositManagement.Domain.Deposits.Events;

namespace Wealth.DepositManagement.Domain.Deposits;

public class Deposit : AggregateRoot
{
    public DepositId Id { get; private set; }
    public string Name { get; private set; }
    public Yield Yield { get; private set; }
    public Money Investment { get; private set; }

    public Money InterestPerYear => Investment * Yield;

    private Deposit()
    {
    }

    public static Deposit Create(string name, Yield yield, CurrencyId currency)
    {
        return Create(DepositId.New(), name, yield, currency);
    }

    public static Deposit Create(DepositId id, string name, Yield yield, CurrencyId currency)
    {
        var deposit = new Deposit
        {
            Id = id,
            Name = name,
            Yield = yield,
            Investment = new Money(currency, 0m),
        };
        deposit.Apply(new DepositCreated(deposit));
        return deposit;
    }

    public void ChangeName(string name)
    {
        if (Name == name)
            return;

        Apply(new DepositRenamed(Id, name));
    }

    public void ChangeYield(Yield yield)
    {
        if (Yield == yield)
            return;

        Apply(new DepositYieldChanged(Id, yield));
    }

    public void Invest(Money investment)
    {
        if (Investment.CurrencyId != investment.CurrencyId)
            throw new InvalidOperationException("Cannot invest in different currencies");

        Apply(new DepositInvested(Id, investment));
    }

    public void Withdraw(Money withdraw)
    {
        if (Investment.CurrencyId != withdraw.CurrencyId)
            throw new InvalidOperationException("Cannot withdraw in different currencies");

        if (Investment.Amount < withdraw.Amount)
            throw new InvalidOperationException("Not enough to withdraw");

        Apply(new DepositWithdrew(Id, withdraw));
    }

    private void When(DepositCreated @event)
    {
        Id = @event.Deposit.Id;
        Name = @event.Deposit.Name;
        Yield = @event.Deposit.Yield;
    }

    private void When(DepositRenamed @event)
    {
        Name = @event.Name;
    }

    private void When(DepositYieldChanged @event)
    {
        Yield = @event.Yield;
    }

    private void When(DepositInvested @event)
    {
        Investment = Investment with { Amount = Investment.Amount + @event.Investment.Amount };
    }

    private void When(DepositWithdrew @event)
    {
        Investment = Investment with { Amount = Investment.Amount - @event.Withdraw.Amount };
    }
}