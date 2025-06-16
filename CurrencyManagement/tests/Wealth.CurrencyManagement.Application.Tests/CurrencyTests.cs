using Wealth.CurrencyManagement.Application.Currency.Commands;
using Wealth.CurrencyManagement.Domain.Currency;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Tests;

public class CurrencyTests
{
    [Fact]
    public async Task WhenCreateCurrency()
    {
        var currencyId = new CurrencyId("FOO");
        var command = new CreateCurrencyCommand(currencyId, "Bar", "Z");
        var repo = Substitute.For<ICurrencyRepository>();
        var handler = new CreateCurrencyCommandHandler(repo);
        
        await handler.Handle(command, CancellationToken.None);

        await repo.Received(1).CreateCurrency(command.CurrencyId, command.Name, command.Symbol);
    }
}