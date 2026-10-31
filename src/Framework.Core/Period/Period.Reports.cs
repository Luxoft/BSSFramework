using System.Globalization;

// ReSharper disable once CheckNamespace
namespace Framework.Core;

public partial struct Period
{
    /// <summary>
    /// Converts the period start date into a string representation of the month and year in the prepositional case for the Russian standard
    /// </summary>
    /// <returns>String containing the full month name and year in the prepositional case</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetInMonthAndYearStrRus instead")]
    public string GetInMonthAndYearStrRus() => CommonPeriodExtensions.GetInMonthAndYearStrRus(this);

    /// <summary>
    /// Converts the start and end dates of the period into a string description of the interval represented by months and year for the Russian standard
    /// </summary>
    /// <returns>String containing the month name and year if the period is within one month, otherwise the names of the start and end months with the year</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetMonthAndYearStrRus instead")]
    public string GetMonthAndYearStrRus() => CommonPeriodExtensions.GetMonthAndYearStrRus(this);

    /// <summary>
    /// Converts the start and end dates of the period into a string description of the interval represented by months and year
    /// </summary>
    /// <returns>String containing the month name and year if the period is within one month, otherwise the names of the start and end months with the year</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetMonthAndYearStr instead")]
    public string GetMonthAndYearStr() => CommonPeriodExtensions.GetMonthAndYearStr(this);

    /// <summary>
    /// Converts the start and end dates of the period into a string description of the interval represented by months and year for the Roman standard
    /// </summary>
    /// <returns>String containing the Roman month numeral and year if the period is within one month, otherwise the Roman numerals of the start and end months with the year</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetMonthAndYearStrRoman instead")]
    public string GetMonthAndYearStrRoman() => CommonPeriodExtensions.GetMonthAndYearStrRoman(this);

    /// <summary>
    /// Returns the week of the year to which the period start date belongs for the current standard
    /// </summary>
    /// <returns>A positive integer representing the week of the year to which the period start date belongs for the current standard</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetWeekNumber instead")]
    public int GetWeekNumber() => CommonPeriodExtensions.GetWeekNumber(this);

    /// <summary>
    /// Returns the week of the year to which the period start date belongs for the <paramref name="isoCultureName"/> standard
    /// </summary>
    /// <param name="isoCultureName">Predefined CultureInfo name, name of an existing CultureInfo object, or a Windows-only culture name, case-insensitive</param>
    /// <returns>A positive integer representing the week of the year to which the period start date belongs for the specified standard</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetWeekNumber instead")]
    public int GetWeekNumber(string isoCultureName) => CommonPeriodExtensions.GetWeekNumber(this, isoCultureName);

    /// <summary>
    /// Converts the start and end dates of the period into a short string description of the interval represented by months and year
    /// </summary>
    /// <returns>String containing the month number and year if the period is within one month, otherwise the numbers of the start and end months with the year</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.ToExtraShortString instead")]
    public string ToExtraShortString() => CommonPeriodExtensions.ToExtraShortString(this);

    /// <summary>
    /// Converts the start and end dates of the period into a short string description of the interval represented by months and year for the <paramref name="cultureInfo"/> regional settings
    /// </summary>
    /// <param name="cultureInfo">Provides information about a specific language and regional settings used to format period dates</param>
    /// <returns>String containing the month number and year if the period is within one month, otherwise the numbers of the start and end months with the year</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.ToExtraShortString instead")]
    public string ToExtraShortString(CultureInfo cultureInfo) => CommonPeriodExtensions.ToExtraShortString(this, cultureInfo);

    /// <summary>
    /// Returns the date format for the <paramref name="cultureInfo"/> regional settings containing only month and year
    /// </summary>
    /// <param name="cultureInfo">Provides information about a specific language and regional settings used to format period dates</param>
    /// <returns>String representing the date format in the specified regional settings for displaying only the month and year</returns>
    [Obsolete("Only for reports. In plain code use CommonPeriodExtensions.GetDayAndMonthDateFormat instead")]
    public string GetDayAndMonthDateFormat(CultureInfo cultureInfo) => CommonPeriodExtensions.GetDayAndMonthDateFormat(this, cultureInfo);
}
