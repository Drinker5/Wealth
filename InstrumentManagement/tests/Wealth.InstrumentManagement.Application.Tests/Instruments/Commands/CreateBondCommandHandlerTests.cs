using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Domain.Instruments;
using Wealth.InstrumentManagement.Domain.Repositories;

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
            ISIN = ISIN.Empty,
            Name = "Foo",
        };
        InstrumentId id = new Guid("00000000-0000-0000-0000-000000000001");
        A.CallTo(() => bondsRepository.CreateBond(command.Name, command.ISIN)).Returns(id);
        var handler = new CreateBondCommandHandler(bondsRepository);

        var result = await handler.Handle(command, CancellationToken.None);

        A.CallTo(() => bondsRepository.CreateBond(command.Name, command.ISIN)).MustHaveHappened();
        Assert.That(result, Is.EqualTo(id));
    }
}