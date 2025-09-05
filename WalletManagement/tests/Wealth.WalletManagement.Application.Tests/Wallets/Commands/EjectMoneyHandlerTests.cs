using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Application.Wallets.Commands;
using Wealth.WalletManagement.Domain.Repositories;

namespace Wealth.WalletManagement.Application.Tests.Wallets.Commands;

[TestSubject(typeof(EjectMoneyHandler))]
public class EjectMoneyHandlerTests
{
    private readonly EjectMoneyHandler handler;
    private readonly Mock<IWalletRepository> walletRepositoryMock = new();

    public EjectMoneyHandlerTests()
    {
        handler = new EjectMoneyHandler(walletRepositoryMock.Object);
    }

    [Fact]
    public async Task WhenHandle()
    {
        var command = new EjectMoney(3, new Money(CurrencyCode.RUB, 23.23m));

        await handler.Handle(command, CancellationToken.None);

        walletRepositoryMock.Verify(i => i.EjectMoney(command.WalletId, command.Money), Times.Once);
    }
}