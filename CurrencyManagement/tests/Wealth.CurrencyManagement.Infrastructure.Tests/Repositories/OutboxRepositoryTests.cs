using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.CurrencyManagement.Application.Abstractions;
using Wealth.CurrencyManagement.Application.Tests.TestHelpers;
using Wealth.CurrencyManagement.Infrastructure.Repositories;
using Wealth.CurrencyManagement.Infrastructure.UnitOfWorks;

namespace Wealth.CurrencyManagement.Infrastructure.Tests.Repositories;

[TestSubject(typeof(DeferredOperationRepository))]
public class DeferredOperationRepositoryTests
{
    private readonly DeferredOperationRepository repo;
    private readonly WealthDbContext context;
    private readonly DateTimeOffset now = new DateTimeOffset(2010, 10, 10, 0, 0, 0, TimeSpan.Zero);

    public DeferredOperationRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<WealthDbContext>()
            .UseInMemoryDatabase("fakeDb")
            .Options;
        context = new WealthDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        repo = new DeferredOperationRepository(context);
        Clock.SetCustomDate(now);
    }

    [Fact]
    public async Task WhenLoadUnprocessed()
    {
        await CreateOutbox();

        var result = await repo.LoadUnprocessed(1, CancellationToken.None);

        Assert.Single(result);
    }

    [Fact]
    public async Task WhenLoadUnprocessed_DelayedInFuture()
    {
        Clock.SetCustomDate(new DateTimeOffset(2010, 10, 10, 0, 0, 0, TimeSpan.Zero));
        await CreateOutbox(
            new OutboxMessageBuilder().SetProcessingDate(now.AddDays(1)).Build());

        var result = await repo.LoadUnprocessed(1, CancellationToken.None);

        Assert.Empty(result);
    }
    
    
    [Fact]
    public async Task WhenLoadUnprocessed_DelayedPastTime()
    {
        Clock.SetCustomDate(new DateTimeOffset(2010, 10, 10, 0, 0, 0, TimeSpan.Zero));
        await CreateOutbox(
            new OutboxMessageBuilder().SetProcessingDate(now.AddDays(-1)).Build());

        var result = await repo.LoadUnprocessed(1, CancellationToken.None);

        Assert.Single(result);
    }

    private async Task<DefferedCommand> CreateOutbox()
    {
        var outboxMessage = new OutboxMessageBuilder().Build();
        return await CreateOutbox(outboxMessage);
    }

    private async Task<DefferedCommand> CreateOutbox(DefferedCommand outboxMessage)
    {
        await repo.Add(outboxMessage);
        await context.SaveChangesAsync();
        return outboxMessage;
    }
}