using Wealth.BuildingBlocks.Application;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

public  readonly record struct UpdateInstruments(IReadOnlyCollection<string> isins) : ICommand;

public sealed class UpdateInstrumentsHandler : ICommandHandler<UpdateInstruments>
{
    public Task Handle(UpdateInstruments request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}