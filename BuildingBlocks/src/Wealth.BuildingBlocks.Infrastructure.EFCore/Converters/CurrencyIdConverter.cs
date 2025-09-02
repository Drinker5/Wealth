using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;

public class CurrencyIdConverter() : ValueConverter<CurrencyId, byte>(v => (byte)v.Value, v => new CurrencyId(v));