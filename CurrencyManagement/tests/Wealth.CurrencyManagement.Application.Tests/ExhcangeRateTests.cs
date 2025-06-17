using Wealth.CurrencyManagement.Application.ExchangeRates.Commands;
using Wealth.CurrencyManagement.Application.ExchangeRates.Query;
using Wealth.CurrencyManagement.Application.Tests.TestHelpers;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.ExchangeRates;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Tests;

public class ExhcangeRateTests
{
    [Fact]
    public async Task WhenCreateCurrency()
    {
        var baseId = new CurrencyId("FOO");
        var toId = new CurrencyId("BAR");
        var date = new DateTime(2000, 1, 1);
        var command = new CreateExchangeRateCommand(baseId, toId, 1.1m, date);
        var repo = Substitute.For<IExchangeRateRepository>();
        var handler = new CreateExchangeRateCommandHandler(repo);

        await handler.Handle(command, CancellationToken.None);

        await repo.Received(1)
            .CreateExchangeRate(
                command.BaseCurrencyId,
                command.TargetCurrencyId,
                command.ExchangeRate,
                command.ValidOnDate);
    }

    [Fact]
    public async Task WhenExchange()
    {
        var money = new Money(new CurrencyId("FOO"), 100);
        var query = new ExchangeQuery(money, new CurrencyId("BAR"), new DateTime(2000, 1, 1));
        var repo = Substitute.For<IExchangeRateRepository>();
        var rate = new ExchangeRateBuilder().Build();
        repo.GetExchangeRate(money.CurrencyId, query.TargetCurrencyId, query.Date).Returns(rate);
        
        var handler = new ExchangeQueryHandler(repo);
        var result = await handler.Handle(query, CancellationToken.None);

        await repo.Received(1).GetExchangeRate(money.CurrencyId, query.TargetCurrencyId, query.Date);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task WhenExchangeIsNotFound()
    {
        var money = new Money(new CurrencyId("FOO"), 100);
        var query = new ExchangeQuery(money, new CurrencyId("BAR"), new DateTime(2000, 1, 1));
        var repo = Substitute.For<IExchangeRateRepository>();
        
        var handler = new ExchangeQueryHandler(repo);
        var result = await handler.Handle(query, CancellationToken.None);

        await repo.Received(1).GetExchangeRate(money.CurrencyId, query.TargetCurrencyId, query.Date);
        Assert.Null(result);
    }
}