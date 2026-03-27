namespace Framework.Core;

public static class DateTimeExtensions
{
    public static DateTime Min(DateTime dateTime1, DateTime dateTime2) => dateTime1 < dateTime2 ? dateTime1 : dateTime2;

    public static DateTime Max(DateTime dateTime1, DateTime dateTime2) => dateTime1 > dateTime2 ? dateTime1 : dateTime2;

    public static DateTime ToStartMonthDate(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, 1);

    public static DateTime ToEndMonthDate(this DateTime dateTime) => dateTime.ToStartMonthDate().AddMonth().SubtractDay();

    public static DateTime ToEndDayDate(this DateTime dateTime) => dateTime.Date.AddDay().AddTicks(-1);

    /// <summary>
    /// Remove Time and TimeZone info from DateTime
    /// </summary>
    public static DateTime ClearTimeZone(this DateTime dateTime) => new(dateTime.Year, dateTime.Month, dateTime.Day);

    public static DateTime ToStartYearDate(this DateTime date) => new(date.Year, 1, 1);

    public static DateTime ToEndYearDate(this DateTime date) => date.ToStartYearDate().AddYear().SubtractDay();

    public static DateTime AddDay(this DateTime dateTime) => dateTime.AddDays(1);

    public static DateTime AddWeek(this DateTime dateTime) => dateTime.AddDays(7);

    public static DateTime AddMonth(this DateTime dateTime) => dateTime.AddMonths(1);

    public static DateTime AddYear(this DateTime dateTime) => dateTime.AddYears(1);

    public static DateTime SubtractDay(this DateTime dateTime) => dateTime.AddDays(-1);

    public static DateTime SubtractWeek(this DateTime dateTime) => dateTime.AddDays(-7);

    public static DateTime SubtractMonth(this DateTime dateTime) => dateTime.AddMonths(-1);

    public static DateTime SubtractYear(this DateTime dateTime) => dateTime.AddYears(-1);

    public static bool IsFirstMonthDate(this DateTime dateTime) => dateTime.Day == 1;

    public static bool IsLastMonthDate(this DateTime dateTime) => dateTime.AddDay().Month != dateTime.Month;

    public static Period ToPeriod(this DateTime startDate, DateTime? endDate = null) => new(startDate, endDate);

    public static Period ToMonth(this DateTime dateTime) => new(dateTime.Year, dateTime.Month);

    public static Period ToYear(this DateTime dateTime) => Period.FromYear(dateTime.Year);

    public static bool LessOrEqualIgnoreTime(this DateTime? _this, DateTime date) => _this < date.AddDay();

    public static int GetQuarter(this DateTime dateTime) => dateTime.Month / 3 + (dateTime.Month % 3 == 0 ? 0 : 1);

    public static string ToShortDateString(this DateTime? value) => value?.ToShortDateString() ?? string.Empty;

    public static string GetMonthAndYear(DateTime? date) => date?.ToString("MM.yyyy") ?? string.Empty;

    private static readonly DateTime MaxValue = System.Data.SqlTypes.SqlDateTime.MaxValue.Value.SubtractDay();

    private static readonly DateTime MinValue = System.Data.SqlTypes.SqlDateTime.MinValue.Value.AddDay();

    public static DateTime ToSqlDateTime(this DateTime value) =>
        value >= MaxValue ? MaxValue
        : value <= MinValue ? MinValue
        : value;

    public static DateTime? ToSqlDateTime(this DateTime? value) => value?.ToSqlDateTime();

    public static int DiffDays(this DateTime start, DateTime end) => (end - start).Days;
}
