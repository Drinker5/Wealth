using Wealth.PortfolioManagement.Application.Portfolios.Commands;
using Wealth.PortfolioManagement.Domain.Repositories;

namespace Wealth.PortfolioManagement.Application.Tests.Portfolios.Commands;

[TestSubject(typeof(CreatePortfolioHandler))]
public class CreatePortfolioHandlerTests
{
    private readonly CreatePortfolioHandler handler;
    private readonly Mock<IPortfolioRepository> repoMock;

    public CreatePortfolioHandlerTests()
    {
        repoMock = new Mock<IPortfolioRepository>();

        handler = new CreatePortfolioHandler(repoMock.Object);
    }

    [Fact]
    public async Task METHOD()
    {
        var command = new CreatePortfolio("foo");
        repoMock.Setup(i => i.CreatePortfolio(command.Name)).ReturnsAsync(3);

        var result = await handler.Handle(command, CancellationToken.None);

        repoMock.Verify(i => i.CreatePortfolio(command.Name), Times.Once);
        Assert.Equal(3, result.Id);
    }
}