using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Domain.Repositories;

namespace Wealth.InstrumentManagement.Application.Tests.Instruments.Commands;

[TestFixture]
[TestOf(typeof(CreateStockCommandHandler))]
public class CreateStockCommandHandlerTests
{
    [Test]
    public async Task WhenHandle()
    {
        var stocksRepository = A.Fake<IStocksRepository>();
        var command = new CreateStockCommand
        {
            ISIN = ISIN.Empty,
            Name = "Foo",
        };
        StockId id = 1;
        A.CallTo(() => stocksRepository.CreateStock(command.Name, command.ISIN)).Returns(id);
        var handler = new CreateStockCommandHandler(stocksRepository);
        
        var result = await handler.Handle(command, CancellationToken.None);
        
        A.CallTo(() => stocksRepository.CreateStock(command.Name, command.ISIN)).MustHaveHappened();
        Assert.That(result, Is.EqualTo(id));
    }
}