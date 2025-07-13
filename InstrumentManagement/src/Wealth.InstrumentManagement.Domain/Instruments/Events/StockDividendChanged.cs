using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Domain.Instruments.Events;

public class StockDividendChanged : IDomainEvent
{
    public InstrumentId Id { get; set; }
    public ISIN ISIN { get; set; }
    public Dividend NewDividend { get; set; }
}