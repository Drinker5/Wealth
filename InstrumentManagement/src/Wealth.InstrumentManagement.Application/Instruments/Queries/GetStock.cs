using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public record struct GetStock(StockId Id) : IQuery<Stock?>;

public record struct GetStockByFigi(FIGI Figi) : IQuery<Stock?>;

public record struct GetStockByIsin(ISIN Isin) : IQuery<Stock?>;

public record struct GetStockByInstrumentUId(InstrumentUId UId) : IQuery<Stock?>;
