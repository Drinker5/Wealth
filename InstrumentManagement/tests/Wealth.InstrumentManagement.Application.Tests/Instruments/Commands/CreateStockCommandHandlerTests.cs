using AutoFixture;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Tests.Instruments.Commands;

[TestFixture]
[TestOf(typeof(CreateStockCommandHandler))]
public class CreateStockCommandHandlerTests
{
    [Test]
    public async Task WhenHandle()
    {
        var stocksRepository = A.Fake<IStocksRepository>();
        var command = GlobalSetup.Fixture.Create<CreateStockCommand>();
        var id = GlobalSetup.Fixture.Create<StockId>();
        A.CallTo(() => stocksRepository.CreateStock(command, CancellationToken.None))
            .Returns(id);
        var handler = new CreateStockCommandHandler(stocksRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => stocksRepository.CreateStock(command, CancellationToken.None))
            .MustHaveHappened();
        Assert.That(result, Is.EqualTo(id));
    }
}