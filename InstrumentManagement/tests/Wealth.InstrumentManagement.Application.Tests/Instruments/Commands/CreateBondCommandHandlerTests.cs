using Wealth.InstrumentManagement.Application.Instruments.Commands;
using Wealth.InstrumentManagement.Application.Services;
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
        var handler = new CreateBondCommandHandler(bondsRepository);
        
        await handler.Handle(command, CancellationToken.None);
        
        A.CallTo(() => bondsRepository.CreateBond(command.Name, command.ISIN)).MustHaveHappened();
    }
}