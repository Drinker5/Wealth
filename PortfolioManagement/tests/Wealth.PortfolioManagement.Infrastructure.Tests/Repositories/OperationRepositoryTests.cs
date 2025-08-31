using Microsoft.EntityFrameworkCore;
using Wealth.PortfolioManagement.Domain.Operations;
using Wealth.PortfolioManagement.Infrastructure.Repositories;
using Wealth.PortfolioManagement.Infrastructure.UnitOfWorks;

namespace Wealth.PortfolioManagement.Infrastructure.Tests.Repositories;

[TestSubject(typeof(OperationRepository))]
public class OperationRepositoryTests
{
    private readonly OperationRepository repository;
    private readonly WealthDbContext context;

    public OperationRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<WealthDbContext>()
            .UseInMemoryDatabase("fakeDb")
            .Options;
        context = new WealthDbContext(options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        repository = new OperationRepository(context);
    }

    [Fact]
    public async Task WhenOperationCreated()
    {
        var id = await repository.CreateOperation(new StockDelistOperation());
        
        Assert.NotEqual(Guid.Empty, id);
    }
    
    [Fact]
    public async Task WhenOperationCreated2()
    {
        var id = await repository.CreateOperation(new CurrencyOperation());
        
        Assert.NotEqual(Guid.Empty, id);
    }
}