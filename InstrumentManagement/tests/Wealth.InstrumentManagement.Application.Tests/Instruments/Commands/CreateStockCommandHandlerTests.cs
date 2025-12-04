using Wealth.BuildingBlocks.Domain.Common;
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
            Index = "FAKE",
            Isin = ISIN.Empty,
            Figi = FIGI.Empty,
            Name = "Foo",
            LotSize = LotSize.One,
        };

        StockId id = 1;
        A.CallTo(() => stocksRepository.CreateStock(
                command.Index,
                command.Name,
                command.Isin,
                command.Figi,
                command.LotSize,
                CancellationToken.None))
            .Returns(id);
        var handler = new CreateStockCommandHandler(stocksRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => stocksRepository.CreateStock(
                command.Index,
                command.Name,
                command.Isin,
                command.Figi,
                command.LotSize,
                CancellationToken.None))
            .MustHaveHappened();
        Assert.That(result, Is.EqualTo(id));
    }
}