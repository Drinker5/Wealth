using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Application.Services;

public interface IInstrumentService
{
    Task<InstrumentInfo> GetInstrumentInfo(InstrumentId instrumentId);
}

public abstract class InstrumentInfo
{
    public InstrumentId Id { get; set; }
    public abstract InstrumentType Type { get; }
    public string Name { get; set; }
    public Money Price { get; set; }
}

public class StockInstrumentInfo : InstrumentInfo
{
    public override InstrumentType Type => InstrumentType.Stock;
    public Money DividendPerYear { get; set; }
    public int LotSize { get; set; }
}

public class BondInstrumentInfo : InstrumentInfo
{
    public override InstrumentType Type => InstrumentType.Stock;
}


public enum InstrumentType
{
    Stock,
    Bond
}