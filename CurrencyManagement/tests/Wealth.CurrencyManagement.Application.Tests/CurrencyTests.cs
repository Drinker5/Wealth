using Wealth.CurrencyManagement.Application.Currencies.Commands;
using Wealth.CurrencyManagement.Application.Currencies.Queries;
using Wealth.CurrencyManagement.Application.Tests.TestHelpers;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Tests;

public class CurrencyTests
{
    [Fact]
    public async Task WhenCreateCurrency()
    {
        var command = new CreateCurrencyCommand("EUR", "Bar", "Z");
        var repo = Substitute.For<ICurrencyRepository>();
        var currency = new CurrencyBuilder().Build();
        repo.CreateCurrency(command.CurrencyId, command.Name, command.Symbol).Returns(currency);
        var handler = new CreateCurrencyCommandHandler(repo);
        
        var result = await handler.Handle(command, CancellationToken.None);

        await repo.Received(1).CreateCurrency(command.CurrencyId, command.Name, command.Symbol);
        Assert.Equal(currency.Id, result.CurrencyId);
        Assert.Equal(currency.Name, result.Name);
        Assert.Equal(currency.Symbol, result.Symbol);
    }

    [Fact]
    public async Task WhenGetCurrencies()
    {
        var repo = Substitute.For<ICurrencyRepository>();
        var currency = new CurrencyBuilder().Build();
        IEnumerable<Currency> currencies = [currency];
        repo.GetCurrencies().Returns(currencies);
        var handler = new GetCurrenciesQueryHandler(repo);
        var query = new GetCurrenciesQuery();
        
        var result = await handler.Handle(query, CancellationToken.None);
        
        await repo.Received(1).GetCurrencies();
        var dto = Assert.Single(result);
        Assert.Equal(currency.Id, dto.CurrencyId);
        Assert.Equal(currency.Name, dto.Name);
        Assert.Equal(currency.Symbol, dto.Symbol);
    }
}