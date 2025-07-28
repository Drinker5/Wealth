using Wealth.WalletManagement.Application.Wallets.Commands;
using Wealth.WalletManagement.Domain.Repositories;

namespace Wealth.WalletManagement.Application.Tests.Wallets.Commands;

[TestSubject(typeof(CreateWalletHandler))]
public class CreateWalletHandlerTests
{
    private readonly CreateWalletHandler handler;
    private readonly Mock<IWalletRepository> walletRepositoryMock = new();

    public CreateWalletHandlerTests()
    {
        handler = new CreateWalletHandler(walletRepositoryMock.Object);
    }

    [Fact]
    public async Task WhenHandle()
    {
        var command = new CreateWallet("foo");

        await handler.Handle(command, CancellationToken.None);

        walletRepositoryMock.Verify(i => i.Create(command.Name), Times.Once);
    }
}