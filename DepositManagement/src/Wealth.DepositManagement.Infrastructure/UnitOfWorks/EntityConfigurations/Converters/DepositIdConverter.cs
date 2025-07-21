using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.DepositManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class DepositIdConverter() : ValueConverter<DepositId, int>(v => v.Id, v => new DepositId(v));