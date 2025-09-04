using Wealth.DepositManagement.Application.Deposits.Commands;
using Wealth.DepositManagement.Domain.Repositories;

namespace Wealth.DepositManagement.Application.Tests.Deposits.Commands;

[TestSubject(typeof(CreateDepositHandler))]
public class CreateDepositOperationHandlerTests
{
    private readonly Mock<IDepositRepository> repository;
    private readonly CreateDepositHandler handler;

    public CreateDepositOperationHandlerTests()
    {
        repository = new Mock<IDepositRepository>();
        handler = new CreateDepositHandler(repository.Object);
    }

    [Fact]
    public async Task WhenHandle()
    {
        var command = new CreateDeposit("foo", 0.3m, "RUB");

        await handler.Handle(command, CancellationToken.None);

        repository.Verify(i => i.Create(command.Name, command.Yield, command.CurrencyId), Times.Once);
    }
}