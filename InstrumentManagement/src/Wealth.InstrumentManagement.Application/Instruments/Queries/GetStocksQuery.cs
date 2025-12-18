using Wealth.BuildingBlocks.Application;
using Wealth.InstrumentManagement.Domain.Instruments;

namespace Wealth.InstrumentManagement.Application.Instruments.Queries;

public record struct GetStocksQuery : IQuery<IReadOnlyCollection<Stock>>;