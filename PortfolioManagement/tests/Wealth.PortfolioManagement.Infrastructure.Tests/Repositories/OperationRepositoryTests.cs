using Microsoft.EntityFrameworkCore;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Infrastructure.Repositories;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Tests.Repositories;

[TestSubject(typeof(OperationRepository))]
public class OperationRepositoryTests
{
    private readonly OperationRepository repository;

    public OperationRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<WealthDbContext>()
            .UseInMemoryDatabase("fakeDb")
            .Options;
        var context = new WealthDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        repository = new OperationRepository(context);
    }

    [Fact(Skip = "not working in memory")]
    public async Task WhenOperationCreated()
    {
        await repository.UpsertOperation(new StockDelistOperation
        {
            Id = "foo",
            Date = DateTimeOffset.Now,
            StockId = 3,
        }, CancellationToken.None);
    }
}