using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct ChangeDividendCommand(StockId StockId, Dividend Dividend) : ICommand;

public class ChangeDividendCommandHandler(IStocksRepository repository) : ICommandHandler<ChangeDividendCommand>
{
    public async Task Handle(ChangeDividendCommand request, CancellationToken cancellationToken)
    {
        await repository.ChangeDividend(request.StockId, request.Dividend);
    }
}