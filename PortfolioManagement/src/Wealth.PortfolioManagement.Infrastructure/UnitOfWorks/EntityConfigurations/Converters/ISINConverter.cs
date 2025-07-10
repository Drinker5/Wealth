using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.PortfolioManagement.Domain.Portfolios;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class ISINConverter() : ValueConverter<ISIN, string>(v => v.Code, v => new ISIN(v));