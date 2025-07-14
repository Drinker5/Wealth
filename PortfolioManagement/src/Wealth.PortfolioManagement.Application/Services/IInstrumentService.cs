using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Application.Services;

public interface IInstrumentService
{
    Task<InstrumentInfo> GetInstrumentInfo(InstrumentId instrumentId);
}

public class InstrumentInfo
{
    public InstrumentId Id { get; set; }
    public InstrumentType Type { get; set; }
    public string Name { get; set; }
}

public enum InstrumentType
{
    Stock,
    Bond
}