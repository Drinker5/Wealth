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

        var contextFactoryMock = new Mock<IDbContextFactory<WealthDbContext>>();
        contextFactoryMock.Setup(i => i.CreateDbContext()).Returns(context);

        repository = new OperationRepository(contextFactoryMock.Object);
    }

    [Fact]
    public async Task WhenOperationCreated()
    {
        await repository.CreateOperation(new StockDelistOperation(), CancellationToken.None);
    }
}