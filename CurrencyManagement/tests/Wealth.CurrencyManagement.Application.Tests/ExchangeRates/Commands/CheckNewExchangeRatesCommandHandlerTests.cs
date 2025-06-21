using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.ExchangeRates.Commands;
using Wealth.CurrencyManagement.Application.Tests.TestHelpers;
using Wealth.CurrencyManagement.Domain.Currencies;
using Wealth.CurrencyManagement.Domain.Repositories;
using Wealth.CurrencyManagement.Domain.Utilities;

namespace Wealth.CurrencyManagement.Application.Tests.ExchangeRates.Commands;

[TestSubject(typeof(CheckNewExchangeRatesCommandHandler))]
public class CheckNewExchangeRatesCommandHandlerTests
{
    private readonly CheckNewExchangeRatesCommandHandler handler;
    private readonly ICurrencyRepository curencyRepo;
    private readonly IExchangeRateRepository exchangeRateRepository;
    private readonly ICommandsScheduler scheduler;
    private readonly CurrencyId c1 = "FOO";
    private readonly CurrencyId c2 = "BAR";
    private readonly DateOnly d1 = new(2020, 1, 1);

    public CheckNewExchangeRatesCommandHandlerTests()
    {
        curencyRepo = Substitute.For<ICurrencyRepository>();
        exchangeRateRepository = Substitute.For<IExchangeRateRepository>();
        scheduler = Substitute.For<ICommandsScheduler>();

        handler = new CheckNewExchangeRatesCommandHandler(
            curencyRepo,
            exchangeRateRepository,
            scheduler);

        curencyRepo.GetCurrency(c1).Returns(new CurrencyBuilder().SetId(c1).Build());
        curencyRepo.GetCurrency(c2).Returns(new CurrencyBuilder().SetId(c2).Build());
        exchangeRateRepository.GetLastExchangeRateDate(c1, c2).Returns(d1);
    }

    [Fact]
    public async Task WhenCommandExecuted()
    {
        var command = new CheckNewExchangeRatesCommand(c1, c2);
        var nowTime = new DateTimeOffset(2020, 1, 5, 0, 0, 0, TimeSpan.Zero);
        Clock.SetCustomDate(nowTime);

        await handler.Handle(command, CancellationToken.None);

        await curencyRepo.Received(1).GetCurrency(c1); 
        await curencyRepo.Received(1).GetCurrency(c2);
        await exchangeRateRepository.Received(1).GetLastExchangeRateDate(c1, c2);
        Assert.Equal(4, scheduler.ReceivedCalls().Count());
        await scheduler.Received(1)
            .ScheduleAsync(new ProvideNewExchangeRateCommand(c1, c2, d1.AddDays(1)), nowTime, CancellationToken.None);
        await scheduler.Received(1)
            .ScheduleAsync(new ProvideNewExchangeRateCommand(c1, c2, d1.AddDays(2)), nowTime.AddSeconds(6), CancellationToken.None);
        await scheduler.Received(1)
            .ScheduleAsync(new ProvideNewExchangeRateCommand(c1, c2, d1.AddDays(3)), nowTime.AddSeconds(12), CancellationToken.None);
        await scheduler.Received(1)
            .ScheduleAsync(new ProvideNewExchangeRateCommand(c1, c2, d1.AddDays(4)), nowTime.AddSeconds(18), CancellationToken.None);
    }
    
    [Fact]
    public async Task WhenCommandExecuted_NoNewExchangeRates()
    {
        var command = new CheckNewExchangeRatesCommand(c1, c2);
        var nowTime = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);
        Clock.SetCustomDate(nowTime);

        await handler.Handle(command, CancellationToken.None);

        await curencyRepo.Received(1).GetCurrency(c1); 
        await curencyRepo.Received(1).GetCurrency(c2);
        await exchangeRateRepository.Received(1).GetLastExchangeRateDate(c1, c2);
        Assert.Empty(scheduler.ReceivedCalls());
    }
}