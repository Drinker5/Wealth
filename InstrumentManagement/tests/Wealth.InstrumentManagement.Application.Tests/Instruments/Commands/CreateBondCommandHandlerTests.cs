using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Tests.Instruments.Commands;

[TestFixture]
[TestOf(typeof(CreateBondCommandHandler))]
public class CreateBondCommandHandlerTests
{
    [Test]
    public async Task WhenHandle()
    {
        var bondsRepository = A.Fake<IBondsRepository>();
        var command = new CreateBondCommand
        {
            Isin = ISIN.Empty,
            Figi = FIGI.Empty,
            Name = "Foo",
        };
        BondId id = 1;
        A.CallTo(() => bondsRepository.CreateBond(command.Name, command.Isin, command.Figi, CancellationToken.None))
            .Returns(id);
        var handler = new CreateBondCommandHandler(bondsRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => bondsRepository.CreateBond(command.Name, command.Isin, command.Figi, CancellationToken.None))
            .MustHaveHappened();
        Assert.That(result, Is.EqualTo(id));
    }
}