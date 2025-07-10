using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class InstrumentIdConverter() : ValueConverter<InstrumentId, Guid>(v => v.Id, v => new InstrumentId(v));