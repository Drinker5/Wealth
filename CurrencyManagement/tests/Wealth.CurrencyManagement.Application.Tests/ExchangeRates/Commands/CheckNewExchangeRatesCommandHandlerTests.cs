using Wealth.BuildingBlocks.Application.CommandScheduler;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.CurrencyManagement.Application.ExchangeRates.Commands;
using Wealth.CurrencyManagement.Application.Tests.TestHelpers;
using Wealth.CurrencyManagement.Domain.Repositories;

namespace Wealth.CurrencyManagement.Application.Tests.ExchangeRates.Commands;

[TestSubject(typeof(CheckNewExchangeRatesCommandHandler))]
public class CheckNewExchangeRatesCommandHandlerTests
{
    private readonly CheckNewExchangeRatesCommandHandler handler;
    private readonly IExchangeRateRepository exchangeRateRepository;
    private readonly ICommandsScheduler scheduler;
    private readonly CurrencyCode c1 = CurrencyCode.Rub;
    private readonly CurrencyCode c2 = CurrencyCode.Usd;
    private readonly CurrencyCode notExisted = CurrencyCode.None;
    private readonly DateOnly d1 = new(2020, 1, 1);

    public CheckNewExchangeRatesCommandHandlerTests()
    {
        exchangeRateRepository = Substitute.For<IExchangeRateRepository>();
        scheduler = Substitute.For<ICommandsScheduler>();

        handler = new CheckNewExchangeRatesCommandHandler(
            exchangeRateRepository,
            scheduler);

        exchangeRateRepository.GetLastExchangeRateDate(c1, c2).Returns(d1);
    }

    [Fact]
    public async Task WhenCommandExecuted()
    {
        var command = new CheckNewExchangeRatesCommand(c1, c2);
        var nowTime = new DateTimeOffset(2020, 1, 5, 0, 0, 0, TimeSpan.Zero);
        Clock.SetCustomDate(nowTime);

        await handler.Handle(command, CancellationToken.None);

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

        await exchangeRateRepository.Received(1).GetLastExchangeRateDate(c1, c2);
        Assert.Empty(scheduler.ReceivedCalls());
    }

    [Fact]
    public async Task WhenCommandExecuted_SameCurrencies()
    {
        var command = new CheckNewExchangeRatesCommand(c1, c1);
        
        await Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>  await handler.Handle(command, CancellationToken.None));
    }
    
    [Fact]
    public async Task WhenCommandExecuted_NotExistedCurrencies()
    {
        var command = new CheckNewExchangeRatesCommand(c1, notExisted);
        
        await Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>  await handler.Handle(command, CancellationToken.None));
        
        var command2 = new CheckNewExchangeRatesCommand(notExisted, c2);
        
        await Assert.ThrowsAnyAsync<InvalidOperationException>(async () =>  await handler.Handle(command2, CancellationToken.None));
    }
}