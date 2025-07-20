using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.DepositManagement.Domain.Deposits;

namespace Wealth.DepositManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class YieldConverter() : ValueConverter<Yield, decimal>(v => v.PercentPerYear, v => new Yield(v));