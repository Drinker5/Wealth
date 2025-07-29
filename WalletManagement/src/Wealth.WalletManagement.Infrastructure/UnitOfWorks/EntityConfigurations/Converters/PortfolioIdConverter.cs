using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.WalletManagement.Domain.Wallets;

namespace Wealth.WalletManagement.Infrastructure.UnitOfWorks.EntityConfigurations.Converters;

public class WalletIdConverter() : ValueConverter<WalletId, int>(v => v.Id, v => new WalletId(v));
