using Wealth.BuildingBlocks.Application;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public record GetInstrumentQuery(InstrumentId Id) : IQuery<InstrumentDTO?>;
