using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Framework.Core;

public static class MonthFormatter
{
    private static readonly DateTimeFormatInfo RusDateTimeFormat = new CultureInfo("ru-RU").DateTimeFormat;

    private static readonly DateTimeFormatInfo EngDateTimeFormat = new CultureInfo("en-US").DateTimeFormat;

    private static readonly string[] RusInMonths =
    [
        "Январе", "Феврале", "Марте", "Апреле", "Мае", "Июне",
                                                           "Июле", "Августе", "Сентябре", "Октябре", "Ноябре", "Декабре"
    ];

    private static readonly string[] RomanMonths =
    [
        "I ", "II ", "III ", "IV ", "V ", "VI ",
                                                           "VII ", "VIII ", "IX ", "X ", "XI ", "XII "
    ];

    /// <summary>
    /// Converts the value of the <paramref name="date"/> argument into a string representation of the month and year in the prepositional case for the Russian standard
    /// </summary>
    /// <param name="date">Date and time</param>
    /// <returns>String containing the full month name and year in the prepositional case</returns>
    public static string GetInMonthAndYearStrRus(this DateTime date) => $"{RusInMonths[date.Month - 1]} {date.Year} г.";

    /// <summary>
    /// Converts the value of the <paramref name="date"/> argument into a string representation of the month and year for the Russian standard
    /// </summary>
    /// <param name="date">Date and time</param>
    /// <returns>String containing the full month name and year</returns>
    public static string GetMonthAndYearStrRus(this DateTime date) => $"{RusDateTimeFormat.GetMonthName(date.Month)} {date.Year} г.";

    /// <summary>
    /// Converts the value of the <paramref name="date"/> argument into a string representation of the month and year for the English standard
    /// </summary>
    /// <param name="date">Date and time</param>
    /// <returns>String containing the full month name and year</returns>
    public static string GetMonthAndYearStr(this DateTime date) => $"{EngDateTimeFormat.GetMonthName(date.Month)} {date.Year}";

    /// <summary>
    /// Converts the value of the <paramref name="date"/> argument into a string representation of the month for the English standard
    /// </summary>
    /// <param name="date">Date and time</param>
    /// <returns>String containing the full month name</returns>
    public static string GetMonthStr(this DateTime date) => EngDateTimeFormat.GetMonthName(date.Month);

    /// <summary>
    /// Converts the value of the <paramref name="date"/> argument into a string representation of the month and year for the Roman standard
    /// </summary>
    /// <param name="date">Date and time</param>
    /// <returns>String containing the Roman numeral of the month and the year</returns>
    public static string GetMonthAndYearStrRoman(this DateTime date) => $"{RomanMonths[date.Month - 1]}'{date:yy}";

    /// <summary>
    /// Converts the value of the <paramref name="period"/> argument into a string representation of the month interval and year for the Roman standard
    /// </summary>
    /// <param name="period">Period that starts and ends in the same year </param>
    /// <returns>String containing the Roman numerals of the start and end months of the period and the year of the period start date</returns>
    public static string GetMonthAndYearStrRoman(Period period) =>
        period.IsWithinOneMonth
            ? period.StartDate.GetMonthAndYearStrRoman()
            : $"{RomanMonths[period.StartDate.Month - 1]}-{RomanMonths[period.EndDateValue.Month - 1]}'{period.StartDate:yy}";
}
