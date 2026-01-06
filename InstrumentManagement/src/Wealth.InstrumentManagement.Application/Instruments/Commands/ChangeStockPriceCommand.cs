using System.Runtime.InteropServices;
using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Application.Repositories;

namespace Wealth.InstrumentManagement.Application.Instruments.Commands;

[StructLayout(LayoutKind.Auto)]
public record struct ChangeStockPriceCommand(StockId StockId, Money Price) : ICommand;

public class ChangeStockPriceCommandHandler(IStocksRepository repository) : ICommandHandler<ChangeStockPriceCommand>
{
    public async Task Handle(ChangeStockPriceCommand request, CancellationToken cancellationToken)
    {
        await repository.ChangePrice(request.StockId, request.Price);
    }
}