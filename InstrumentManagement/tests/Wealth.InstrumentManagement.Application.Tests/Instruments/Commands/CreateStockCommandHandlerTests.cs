using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Domain.Instruments;
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
        var handler = new CreateStockCommandHandler(stocksRepository);
        
        var id = await handler.Handle(command, CancellationToken.None);
        
        A.CallTo(() => stocksRepository.CreateStock(command.Name, command.ISIN)).MustHaveHappened();
        Assert.That(id, Is.Not.Default);
    }
}