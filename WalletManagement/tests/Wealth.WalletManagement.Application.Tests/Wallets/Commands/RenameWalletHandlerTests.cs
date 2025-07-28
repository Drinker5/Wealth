using Wealth.WalletManagement.Application.Wallets.Commands;
using Wealth.WalletManagement.Domain.Repositories;

namespace Wealth.WalletManagement.Application.Tests.Wallets.Commands;

[TestSubject(typeof(RenameWalletHandler))]
public class RenameWalletHandlerTests
{
    private readonly RenameWalletHandler handler;
    private readonly Mock<IWalletRepository> walletRepositoryMock = new();

    public RenameWalletHandlerTests()
    {
        handler = new RenameWalletHandler(walletRepositoryMock.Object);
    }

    [Fact]
    public async Task WhenHandle()
    {
        var command = new RenameWallet("foo");

        await handler.Handle(command, CancellationToken.None);

        walletRepositoryMock.Verify(i => i.Rename(command.NewName), Times.Once);
    }
}