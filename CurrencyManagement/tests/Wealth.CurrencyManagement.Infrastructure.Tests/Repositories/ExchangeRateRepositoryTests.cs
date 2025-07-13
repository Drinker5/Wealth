using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.CurrencyManagement.Application.Tests.TestHelpers;
using Wealth.CurrencyManagement.Domain.ExchangeRates;
using Wealth.CurrencyManagement.Infrastructure.Repositories;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

namespace Wealth.CurrencyManagement.Infrastructure.Tests.Repositories;

[TestSubject(typeof(ExchangeRateRepository))]
public class ExchangeRateRepositoryTests
{
    private readonly WealthDbContext context;
    private readonly ExchangeRateRepository repo;
    private readonly CurrencyId c1 = "FOO";
    private readonly CurrencyId c2 = "BAR";
    private readonly CurrencyId c3 = "BAZ";
    private readonly DateOnly defaultDate = new DateOnly(2018, 12, 31);

    public ExchangeRateRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<WealthDbContext>()
            .UseInMemoryDatabase("fakeDb")
            .Options;
        context = new WealthDbContext(options);
        context.Database.EnsureDeleted();
        repo = new ExchangeRateRepository(context);
    }

    [Fact]
    public async Task WhenGetLastExchangeRateDate_NoDate()
    {
        var result = await repo.GetLastExchangeRateDate(c1, c2);

        Assert.Equal(defaultDate, result);
    }
    
    [Fact]
    public async Task WhenGetLastExchangeRateDate_NoDate2()
    {
        var date1 = new DateOnly(2021, 3, 4);
        AddExchange(new ExchangeRateBuilder().SetBaseCurrencyId(c1).SetTargetCurrencyId(c3).SetDate(date1).Build());

        var result = await repo.GetLastExchangeRateDate(c1, c2);
    
        Assert.Equal(defaultDate, result);
    }

    [Fact]
    public async Task WhenGetLastExchangeRateDate_HasDate()
    {
        var date1 = new DateOnly(2021, 3, 4);
        var date2 = new DateOnly(2022, 5, 6);
        AddExchange(new ExchangeRateBuilder().SetBaseCurrencyId(c1).SetTargetCurrencyId(c2).SetDate(date1).Build());
        AddExchange(new ExchangeRateBuilder().SetBaseCurrencyId(c1).SetTargetCurrencyId(c2).SetDate(date2).Build());

        var result = await repo.GetLastExchangeRateDate(c1, c2);

        Assert.Equal(date2, result);
    }

    private void AddExchange(ExchangeRate exchangeRate)
    {
        context.ExchangeRates.Add(exchangeRate);
        context.SaveChanges();
    }
}