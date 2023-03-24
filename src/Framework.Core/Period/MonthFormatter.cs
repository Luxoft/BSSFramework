using System;
using System.Globalization;

namespace Framework.Core;

public static class MonthFormatter
{
    private static readonly DateTimeFormatInfo RusDateTimeFormat = new CultureInfo("ru-RU").DateTimeFormat;

    private static readonly DateTimeFormatInfo EngDateTimeFormat = new CultureInfo("en-US").DateTimeFormat;

    private static readonly string[] RusInMonths = new[]
                                                   {
                                                           "Январе", "Феврале", "Марте", "Апреле", "Мае", "Июне",
                                                           "Июле", "Августе", "Сентябре", "Октябре", "Ноябре", "Декабре"
                                                   };

    private static readonly string[] RomanMonths = new[]
                                                   {
                                                           "I ", "II ", "III ", "IV ", "V ", "VI ",
                                                           "VII ", "VIII ", "IX ", "X ", "XI ", "XII "
                                                   };

    /// <summary>
    /// Преобразует значение аргумента <see cref="date"/> в строковое представление месяца и года в предложном падеже для Российского стандарта
    /// </summary>
    /// <param name="date">Дата и время</param>
    /// <returns>Строка, содержащая полное имя месяца и год в предложном падеже</returns>
    public static string GetInMonthAndYearStrRus(this DateTime date)
    {
        return $"{RusInMonths[date.Month - 1]} {date.Year} г.";
    }

    /// <summary>
    /// Преобразует значение аргумента <see cref="date"/> в строковое представление месяца и года для Российского стандарта
    /// </summary>
    /// <param name="date">Дата и время</param>
    /// <returns>Строка, содержащая полное имя месяца и год</returns>
    public static string GetMonthAndYearStrRus(this DateTime date)
    {
        return $"{RusDateTimeFormat.GetMonthName(date.Month)} {date.Year} г.";
    }

    /// <summary>
    /// Преобразует значение аргумента <see cref="date"/> в строковое представление месяца и года для Английского стандарта
    /// </summary>
    /// <param name="date">Дата и время</param>
    /// <returns>Строка, содержащая полное имя месяца и год</returns>
    public static string GetMonthAndYearStr(this DateTime date)
    {
        return $"{EngDateTimeFormat.GetMonthName(date.Month)} {date.Year}";
    }

    /// <summary>
    /// Преобразует значение аргумента <see cref="date"/> в строковое представление месяца для Английского стандарта
    /// </summary>
    /// <param name="date">Дата и время</param>
    /// <returns>Строка, содержащая полное имя месяца</returns>
    public static string GetMonthStr(this DateTime date)
    {
        return EngDateTimeFormat.GetMonthName(date.Month);
    }

    /// <summary>
    /// Преобразует значение аргумента <see cref="date"/> в строковое представление месяца и года для Римского стандарта
    /// </summary>
    /// <param name="date">Дата и время</param>
    /// <returns>Строка, содержащая римский номер месяца и год</returns>
    public static string GetMonthAndYearStrRoman(this DateTime date)
    {
        return $"{RomanMonths[date.Month - 1]}'{date:yy}";
    }

    /// <summary>
    /// Преобразует значение аргумента <see cref="period"/> в строковое представление интервала месяцев и года для Римского стандарта
    /// </summary>
    /// <param name="period">Период, который начинается и заканчивается в одинаковом году </param>
    /// <returns>Строка, содержащая римские номера месяцев начало и конца периода и год на дату начала периода</returns>
    public static string GetMonthAndYearStrRoman<T>(T period) where T : IPeriod
    {
        return period.IsWithinOneMonth
                       ? GetMonthAndYearStrRoman(period.StartDate)
                       : $"{RomanMonths[period.StartDate.Month - 1]}-{RomanMonths[period.EndDateValue.Month - 1]}'{period.StartDate:yy}";
    }
}
