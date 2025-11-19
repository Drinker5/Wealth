using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Application.ExchangeRates.Commands;
using Wealth.CurrencyManagement.Application.ExchangeRates.Queries;
using Wealth.CurrencyManagement.Application.Tests.TestHelpers;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Tests;

public class ExhcangeRateTests
{
    private static readonly CurrencyId rub = CurrencyCode.Rub;
    private static readonly CurrencyId usd = CurrencyCode.Usd;
    private readonly ICurrencyRepository currencyRepo;
    private readonly IExchangeRateRepository exchangeRateRepo;

    public ExhcangeRateTests()
    {
        currencyRepo = Substitute.For<ICurrencyRepository>();
        exchangeRateRepo = Substitute.For<IExchangeRateRepository>();
    }

    [Fact]
    public async Task WhenCreateCurrency()
    {
        var baseId = rub;
        var toId = usd;
        var date = new DateOnly(2000, 1, 1);
        var command = new CreateExchangeRateCommand(baseId, toId, 1.1m, date);
        currencyRepo.GetCurrency(baseId).Returns(new CurrencyBuilder().Build());
        currencyRepo.GetCurrency(toId).Returns(new CurrencyBuilder().Build());
        var handler = new CreateExchangeRateCommandHandler(currencyRepo, exchangeRateRepo);

        await handler.Handle(command, CancellationToken.None);

        await currencyRepo.Received(1).GetCurrency(baseId);
        await currencyRepo.Received(1).GetCurrency(toId);
        await exchangeRateRepo.Received(1)
            .CreateExchangeRate(
                command.BaseCurrencyId,
                command.TargetCurrencyId,
                command.ExchangeRate,
                command.ValidOnDate);
    }

    [Fact]
    public async Task WhenExchange()
    {
        var money = new Money(rub, 100);
        var query = new ExchangeQuery(money, usd, new DateOnly(2000, 1, 1));
        var rate = new ExchangeRateBuilder().Build();
        exchangeRateRepo.GetExchangeRate(money.Currency, query.TargetCurrencyId, query.Date).Returns(rate);

        var handler = new ExchangeQueryHandler(exchangeRateRepo);
        var result = await handler.Handle(query, CancellationToken.None);

        await exchangeRateRepo.Received(1).GetExchangeRate(money.Currency, query.TargetCurrencyId, query.Date);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task WhenExchangeIsNotFound()
    {
        var money = new Money(rub, 100);
        var query = new ExchangeQuery(money, usd, new DateOnly(2000, 1, 1));

        var handler = new ExchangeQueryHandler(exchangeRateRepo);
        var result = await handler.Handle(query, CancellationToken.None);

        await exchangeRateRepo.Received(1).GetExchangeRate(money.Currency, query.TargetCurrencyId, query.Date);
        Assert.Null(result);
    }
}