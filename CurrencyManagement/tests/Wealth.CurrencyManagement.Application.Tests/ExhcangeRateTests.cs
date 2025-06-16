using Wealth.CurrencyManagement.Application.Currency.Commands;
using Wealth.CurrencyManagement.Application.ExchangeRate.Commands;
using Wealth.CurrencyManagement.Domain.Currency;
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
}