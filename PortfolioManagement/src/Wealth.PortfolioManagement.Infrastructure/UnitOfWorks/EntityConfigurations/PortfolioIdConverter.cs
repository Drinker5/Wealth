using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

public class PortfolioIdConverter() : ValueConverter<PortfolioId, Guid>(v => v.Id, v => new PortfolioId(v));