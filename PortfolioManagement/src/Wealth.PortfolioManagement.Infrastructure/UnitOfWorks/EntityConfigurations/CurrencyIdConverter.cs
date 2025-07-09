using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations;

public class CurrencyIdConverter() : ValueConverter<CurrencyId, string>(v => v.Code, v => new CurrencyId(v));