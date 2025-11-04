using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class PortfolioIdConverter() : ValueConverter<PortfolioId, int>(v => v.Value, v => new PortfolioId(v));