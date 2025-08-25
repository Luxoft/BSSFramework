using System.Globalization;

namespace Framework.Core;

public static class DateTimeExtensions
{
    private static readonly DateTimeFormatInfo EngDateTimeFormat = new CultureInfo("en-US").DateTimeFormat;

    public static DateTime AddDay(this DateTime dateTime)
    {
        return dateTime.AddDays(1);
    }

    public static bool IsFirstMonthDate(this DateTime dateTime)
    {
        return dateTime.Day == 1;
    }

    public static bool IsLastMonthDate(this DateTime dateTime)
    {
        return dateTime.AddDay().Month != dateTime.Month;
    }

    public static string GetMonthStr(this DateTime date)
    {
        return EngDateTimeFormat.GetMonthName(date.Month);
    }
}
