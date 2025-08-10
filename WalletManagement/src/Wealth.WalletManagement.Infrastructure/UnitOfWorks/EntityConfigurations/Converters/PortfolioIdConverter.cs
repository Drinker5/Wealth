using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.WalletManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class WalletIdConverter() : ValueConverter<WalletId, int>(v => v.Id, v => new WalletId(v));
