using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Application.ExchangeRates.Commands;
using Wealth.CurrencyManagement.Application.ExchangeRates.Queries;
using Wealth.CurrencyManagement.Application.Tests.TestHelpers;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Tests;

public class ExhcangeRateTests
{
    private const CurrencyCode rub = CurrencyCode.Rub;
    private const CurrencyCode usd = CurrencyCode.Usd;
    private readonly IExchangeRateRepository exchangeRateRepo;

    public ExhcangeRateTests()
    {
        exchangeRateRepo = Substitute.For<IExchangeRateRepository>();
    }

    [Fact]
    public async Task WhenCreateCurrency()
    {
        var baseId = rub;
        var toId = usd;
        var date = new DateOnly(2000, 1, 1);
        var command = new CreateExchangeRateCommand(baseId, toId, 1.1m, date);
        var handler = new CreateExchangeRateCommandHandler(exchangeRateRepo);

        await handler.Handle(command, CancellationToken.None);

        await exchangeRateRepo.Received(1)
            .CreateExchangeRate(
                command.BaseCurrency,
                command.TargetCurrency,
                command.ExchangeRate,
                command.ValidOnDate);
    }

    [Fact]
    public async Task WhenExchange()
    {
        var money = new Money(rub, 100);
        var query = new ExchangeQuery(money, usd, new DateOnly(2000, 1, 1));
        var rate = new ExchangeRateBuilder().Build();
        exchangeRateRepo.GetExchangeRate(money.Currency, query.TargetCurrency, query.Date).Returns(rate);

        var handler = new ExchangeQueryHandler(exchangeRateRepo);
        var result = await handler.Handle(query, CancellationToken.None);

        await exchangeRateRepo.Received(1).GetExchangeRate(money.Currency, query.TargetCurrency, query.Date);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task WhenExchangeIsNotFound()
    {
        var money = new Money(rub, 100);
        var query = new ExchangeQuery(money, usd, new DateOnly(2000, 1, 1));

        var handler = new ExchangeQueryHandler(exchangeRateRepo);
        var result = await handler.Handle(query, CancellationToken.None);

        await exchangeRateRepo.Received(1).GetExchangeRate(money.Currency, query.TargetCurrency, query.Date);
        Assert.Null(result);
    }
}