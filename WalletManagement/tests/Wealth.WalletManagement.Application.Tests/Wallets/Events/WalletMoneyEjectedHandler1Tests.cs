using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Application.Wallets.Events;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Application.Tests.Wallets.Events;

[TestSubject(typeof(WalletMoneyEjectedHandler1))]
public class WalletMoneyEjectedHandler1Tests
{
    private readonly WalletMoneyEjectedHandler1 handler;
    private readonly Mock<IOutboxRepository> repositoryMock = new();

    public WalletMoneyEjectedHandler1Tests()
    {
        handler = new WalletMoneyEjectedHandler1(repositoryMock.Object);
    }

    [Fact]
    public async Task WhenHandle()
    {
        var notification = new WalletMoneyEjected(3, new Money("FOO", 23.23m));

        await handler.Handle(notification, CancellationToken.None);

        repositoryMock.Verify(i => i.Add(It.IsAny<OutboxMessage>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}