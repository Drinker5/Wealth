using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Wealth.BuildingBlocks.Infrastructure.EFCore.Converters;

public class NoConverter<T>() : ValueConverter<T, T>(v => v, v => v);