using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public record struct GetCurrency(CurrencyId Id) : IQuery<Currency?>;

public record struct GetCurrencyByFigi(FIGI Figi) : IQuery<Currency?>;

public record struct GetCurrencyByInstrumentUId(InstrumentUId UId) : IQuery<Currency?>;
