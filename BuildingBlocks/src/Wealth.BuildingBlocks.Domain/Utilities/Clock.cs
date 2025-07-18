﻿namespace Wealth.BuildingBlocks.Domain.Utilities;

public static class Clock
{
    private static DateTimeOffset? _customDate;

    public static DateTimeOffset Now => _customDate ?? DateTime.UtcNow;
    public static DateOnly Today => DateOnly.FromDateTime(_customDate?.DateTime ?? DateTime.UtcNow);

    public static void SetCustomDate(DateTimeOffset customDate)
    {
        _customDate = customDate;
    }

    public static void Reset()
    {
        _customDate = null;
    }
}