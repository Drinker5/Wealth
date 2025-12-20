using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public record struct GetBond(BondId Id) : IQuery<Bond?>;

public record struct GetBondByFigi(FIGI Figi) : IQuery<Bond?>;

public record struct GetBondByIsin(ISIN Isin) : IQuery<Bond?>;

public record struct GetBondByInstrumentId(InstrumentId Id) : IQuery<Bond?>;
