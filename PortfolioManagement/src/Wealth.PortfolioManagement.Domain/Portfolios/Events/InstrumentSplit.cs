
using Wealth.BuildingBlocks.Domain;
using Wealth.BuildingBlocks.Domain.Common;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Domain.Portfolios.Events;

public record InstrumentSplit(
    PortfolioId PortfolioId,
    InstrumentId InstrumentId,
    SplitRatio Ratio) : IDomainEvent;