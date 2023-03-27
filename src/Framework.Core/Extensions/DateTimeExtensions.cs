namespace Framework.Core;

public static class DateTimeExtensions
{
    public static DateTime Min(DateTime dateTime1, DateTime dateTime2)
    {
        return dateTime1 < dateTime2 ? dateTime1 : dateTime2;
    }

    public static DateTime Max(DateTime dateTime1, DateTime dateTime2)
    {
        return dateTime1 > dateTime2 ? dateTime1 : dateTime2;
    }

    public static DateTime ToStartMonthDate(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, 1);
    }

    public static DateTime ToEndMonthDate(this DateTime dateTime)
    {
        return dateTime.ToStartMonthDate().AddMonth().SubtractDay();
    }

    public static DateTime ToEndDayDate(this DateTime dateTime)
    {
        return dateTime.Date.AddDay().AddTicks(-1);
    }

    /// <summary>
    /// Remove Time and TimeZone info from DateTime
    /// </summary>
    public static DateTime ClearTimeZone(this DateTime dateTime)
    {
        return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    public static DateTime ToStartYearDate(this DateTime date)
    {
        return new DateTime(date.Year, 1, 1);
    }

    public static DateTime ToEndYearDate(this DateTime date)
    {
        return date.ToStartYearDate().AddYear().SubtractDay();
    }

    public static DateTime AddDay(this DateTime dateTime)
    {
        return dateTime.AddDays(1);
    }

    public static DateTime AddWeek(this DateTime dateTime)
    {
        return dateTime.AddDays(7);
    }

    public static DateTime AddMonth(this DateTime dateTime)
    {
        return dateTime.AddMonths(1);
    }

    public static DateTime AddYear(this DateTime dateTime)
    {
        return dateTime.AddYears(1);
    }

    public static DateTime SubtractDay(this DateTime dateTime)
    {
        return dateTime.AddDays(-1);
    }

    public static DateTime SubtractWeek(this DateTime dateTime)
    {
        return dateTime.AddDays(-7);
    }

    public static DateTime SubtractMonth(this DateTime dateTime)
    {
        return dateTime.AddMonths(-1);
    }

    public static DateTime SubtractYear(this DateTime dateTime)
    {
        return dateTime.AddYears(-1);
    }

    public static bool IsFirstMonthDate(this DateTime dateTime)
    {
        return dateTime.Day == 1;
    }

    [Obsolete("v10 This is application logic, not a framework")]
    public static bool IsStartFinancialYearDate(this DateTime dateTime)
    {
        return dateTime.Day == 1 && dateTime.Month == Period.FinancialYearStartMonth;
    }

    public static bool IsLastMonthDate(this DateTime dateTime)
    {
        return dateTime.AddDay().Month != dateTime.Month;
    }

    public static Period ToPeriod(this DateTime startDate, DateTime? endDate = null)
    {
        return new Period(startDate, endDate);
    }

    public static Period ToMonth(this DateTime dateTime)
    {
        return new Period(dateTime.Year, dateTime.Month);
    }

    public static Period ToYear(this DateTime dateTime)
    {
        return Period.FromYear(dateTime.Year);
    }

    [Obsolete("v10 This is application logic, not a framework")]
    public static Period ToFinancialYearPeriod(this DateTime date)
    {
        var currentYearFinDate = new DateTime(date.Year, Period.FinancialYearStartMonth, 1);

        return date >= currentYearFinDate
                       ? new Period(currentYearFinDate, currentYearFinDate.AddYear().SubtractDay())
                       : new Period(currentYearFinDate.SubtractYear(), currentYearFinDate.SubtractDay());
    }

    [Obsolete("v10 This is application logic, not a framework")]
    public static Period ToNextFinancialYearPeriod(this DateTime date)
    {
        var currentYearFinDate = new DateTime(date.Year, Period.FinancialYearStartMonth, 1);

        if (date >= currentYearFinDate)
        {
            return new Period(currentYearFinDate.AddYear(), currentYearFinDate.AddYear().AddYear().SubtractDay());
        }

        return new Period(currentYearFinDate, currentYearFinDate.AddYear().SubtractDay());
    }

    public static bool LessOrEqualIgnoreTime(this DateTime? _this, DateTime date)
    {
        return _this < date.AddDay();
    }

    public static int GetQuarter(this DateTime dateTime)
    {
        return dateTime.Month / 3 + (dateTime.Month % 3 == 0 ? 0 : 1);
    }

    public static string ToShortDateString(this DateTime? value)
    {
        return value?.ToShortDateString() ?? string.Empty;
    }

    public static string GetMonthAndYear(DateTime? date)
    {
        return date?.ToString("MM.yyyy") ?? string.Empty;
    }

    private static readonly DateTime MaxValue = System.Data.SqlTypes.SqlDateTime.MaxValue.Value.SubtractDay();

    private static readonly DateTime MinValue = System.Data.SqlTypes.SqlDateTime.MinValue.Value.AddDay();

    public static DateTime ToSqlDateTime(this DateTime value)
    {
        return value >= MaxValue ? MaxValue
               : value <= MinValue ? MinValue
               : value;
    }

    public static DateTime? ToSqlDateTime(this DateTime? value)
    {
        return value?.ToSqlDateTime();
    }

    public static int DiffDays(this DateTime start, DateTime end) => (end - start).Days;
}
