using Wealth.BuildingBlocks.Domain.Common;
using Wealth.WalletManagement.Application.Wallets.Commands;
using Wealth.WalletManagement.Domain.Repositories;

namespace Wealth.WalletManagement.Application.Tests.Wallets.Commands;

[TestSubject(typeof(InsertMoneyHandler))]
public class InsertMoneyHandlerTests
{
    private readonly InsertMoneyHandler handler;
    private readonly Mock<IWalletRepository> walletRepositoryMock = new();

    public InsertMoneyHandlerTests()
    {
        handler = new InsertMoneyHandler(walletRepositoryMock.Object);
    }

    [Fact]
    public async Task WhenHandle()
    {
        var command = new InsertMoney(3, new Money(CurrencyCode.RUB, 23.23m));

        await handler.Handle(command, CancellationToken.None);

        walletRepositoryMock.Verify(i => i.InsertMoney(command.WalletId, command.Money), Times.Once);
    }
}