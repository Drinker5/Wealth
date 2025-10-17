using Microsoft.EntityFrameworkCore;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.BuildingBlocks.Domain.Utilities;
using Wealth.DepositManagement.Domain.DepositOperations;
using Wealth.DepositManagement.Infrastructure.Repositories;
using Wealth.DepositManagement.Infrastructure.UnitOfWorks;

namespace Wealth.DepositManagement.Infrastructure.Tests.Repositories;

[TestSubject(typeof(DepositOperationRepository))]
public class DepositOperationRepositoryTests
{
    private readonly DepositOperationRepository repository;
    private readonly WealthDbContext context;

    public DepositOperationRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<WealthDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        context = new WealthDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        repository = new DepositOperationRepository(context);
    }

    [Fact]
    public async Task WhenOperationCreated()
    {
        var id = await repository.CreateOperation(2, DepositOperationType.Investment, Clock.Now, new Money("RUB", 2));

        Assert.NotEqual(Guid.Empty, id);
    }
}