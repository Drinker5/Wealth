using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.PortfolioManagement.Domain;
using Wealth.PortfolioManagement.Domain.Portfolios;
using Wealth.PortfolioManagement.Domain.ValueObjects;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class CurrencyIdConverter() : ValueConverter<CurrencyId, string>(v => v.Code, v => new CurrencyId(v));