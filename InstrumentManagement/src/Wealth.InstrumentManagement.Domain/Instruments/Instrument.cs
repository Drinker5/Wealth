using Wealth.BuildingBlocks.Domain;

namespace Wealth.InstrumentManagement.Domain.Instruments;

public abstract class Instrument : AggregateRoot
{
    public InstrumentId Id { get; protected set; }

    public string Name { get; protected set; }

    public ISIN ISIN { get; protected set; }

    public Money Price { get; protected set; }
}