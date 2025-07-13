using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Wealth.BuildingBlocks.Domain.Common;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;

public class CurrencyIdConverter() : ValueConverter<CurrencyId, string>(v => v.Code, v => new CurrencyId(v));