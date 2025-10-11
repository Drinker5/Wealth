using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.PortfolioManagement.Domain.Operations;

namespace Wealth.PortfolioManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class OperationIdConverter() : ValueConverter<OperationId, string>(v => v.Value, v => new OperationId(v));