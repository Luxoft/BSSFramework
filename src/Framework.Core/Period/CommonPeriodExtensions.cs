using System.Globalization;

using Anch.Core;

// ReSharper disable once CheckNamespace
namespace Framework.Core;

/// <summary>
/// Extension methods for types implementing the Period interface
/// </summary>
public static class CommonPeriodExtensions
{
    /// <param name="period">Period in which the intersection is checked</param>
    extension(Period period)
    {
        /// <summary>
        /// Returns a value indicating whether the specified period <paramref name="period"/> contains the <paramref name="target"/> period passed as a parameter
        /// </summary>
        /// <remarks>
        /// The comparison takes the sql database limit into account
        /// </remarks>
        /// <param name="target">Period to check the intersection with</param>
        /// <returns>true if the start and end dates of the given period are within the specified period; otherwise false</returns>
        public bool Contains(Period target)
            =>
                period.Contains(target.StartDate) && period.Contains(target.EndDateValue);

        /// <summary>
        /// Returns a value indicating whether the specified period <paramref name="period"/> contains the <paramref name="date"/> date passed as a parameter
        /// </summary>
        /// <remarks>
        /// The comparison takes the sql database limit into account
        /// </remarks>
        /// <param name="date">Date to check for inclusion in the period</param>
        /// <returns>true if the date is within the period; otherwise false</returns>
        public bool Contains(DateTime date)
            => period.StartDate <= date && (period.EndDate == null || date <= period.EndDateValue);
    }

    /// <param name="period">Period in which the date is checked</param>
    extension(Period period)
    {
        /// <summary>
        /// Returns a value indicating whether the specified period <paramref name="period"/> contains the <paramref name="date"/> date passed as a parameter
        /// </summary>
        /// <remarks>
        /// The comparison takes the sql database limit into account
        /// </remarks>
        /// <param name="date">Date to check for inclusion in the period</param>
        /// <returns>true if the date is not null and is within the period; otherwise false</returns>
        public bool ContainsExt(DateTime? date)
            => date.MaybeNullable(v => period.Contains(v));

        /// <summary>
        /// Returns a value indicating whether the specified period <paramref name="period"/> contains the <paramref name="date"/> date passed as a parameter
        /// </summary>
        /// <param name="date">Date to check for inclusion in the period</param>
        /// <returns>true if the date is within the period; otherwise false</returns>
        public bool NativeContains(DateTime date) => period.NativeStartDate <= date && (period.NativeEndDate is null || date <= period.NativeEndDateValue);

        /// <summary>
        /// Returns a value indicating whether the specified period <paramref name="period"/> contains the <paramref name="date"/> date passed as a parameter
        /// </summary>
        /// <param name="date">Date to check for inclusion in the period</param>
        /// <returns>true if the date is not null and is within the period; otherwise false</returns>
        public bool NativeContains(DateTime? date) => date.MaybeNullable(z => period.NativeContains(z));

        /// <summary>
        /// Returns a value indicating whether the specified period <paramref name="period"/>, excluding the period end date, contains the <paramref name="date"/> date passed as a parameter
        /// </summary>
        /// <remarks>
        /// The comparison takes the sql database limit into account
        /// </remarks>
        /// <param name="date">Date to check for inclusion in the period</param>
        /// <returns>true if the date is within the period and is not equal to the period end date; otherwise false</returns>
        public bool ContainsWithoutEndDate(DateTime date) => period.StartDate <= date && (period.EndDate == null || date < period.EndDateValue);

        /// <summary>
        /// Returns a list of dates in the period, aligned to the working part of the week
        /// (i.e. for a week, dates corresponding to the period from Monday to Friday are returned)
        /// </summary>
        public IEnumerable<DateTime?> DatesByWorkingWeek()
        {
            if (period.EndDate == null)
            {
                throw new ArgumentException("this operation cannot be performed for the given object");
            }

            var result = new List<DateTime?>((period.EndDateValue - period.StartDate).Days);

            var current = period.StartDate;

            while (current <= period.EndDateValue)
            {
                if (current.DayOfWeek != DayOfWeek.Saturday && current.DayOfWeek != DayOfWeek.Sunday)
                {
                    result.Add(current);
                }

                current = current.AddDay();
            }

            return result;
        }

        /// <summary>
        /// Returns the number of whole months in the period
        /// </summary>
        /// <returns>If EndDate is null or EndDate is greater than StartDate, returns null; otherwise the number of months</returns>
        public int? TotalMonths()
        {
            if (!period.EndDate.HasValue || period.EndDate < period.StartDate)
            {
                return null;
            }

            return (period.EndDate.Value.Year - period.StartDate.Year) * 12
                   + period.EndDate.Value.Month
                   - period.StartDate.Month;
        }

        /// <summary>
        /// Returns the string representation of the current period <paramref name="period"/> for the regional settings <paramref name="cultureInfo"/>
        /// </summary>
        /// <param name="cultureInfo">Regional settings in which the period must be displayed</param>
        /// <param name="displayYear">Flag indicating whether to output the year in the string representation of the period</param>
        /// <returns>String representing the period in a display-friendly form</returns>
        public string ToDisplayName(CultureInfo cultureInfo, bool displayYear = false)
        {
            if (period.IsEmpty)
            {
                return string.Empty;
            }

            if (period.EndDate == null)
            {
                return $"{period.StartDate.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo)}-∞";
            }

            if (period.StartDate.Year != period.EndDateValue.Year)
            {
                return
                    $"{period.StartDate.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo)}-{period.EndDateValue.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo)}";
            }

            if (period.StartDate.Month != period.EndDateValue.Month)
            {
                var shortFormat = period.GetDayAndMonthDateFormat(cultureInfo);

                return period.ProcessYear($"{period.StartDate.ToString(shortFormat, cultureInfo)}-{period.EndDateValue.ToString(shortFormat, cultureInfo)}", displayYear);
            }

            var monthStr = cultureInfo.DateTimeFormat.GetMonthName(period.StartDate.Month);

            if (period.StartDate.IsFirstMonthDate() && period.EndDateValue.IsLastMonthDate())
            {
                return period.ProcessYear(monthStr, displayYear);
            }

            if (period.StartDate.Day == period.EndDateValue.Day)
            {
                return period.ProcessYear($"{period.StartDate:dd} {monthStr}", displayYear);
            }

            return period.ProcessYear($"{period.StartDate:dd}-{period.EndDateValue:dd} {monthStr}", displayYear);
        }

        /// <summary>
        /// Returns the string representation of the current period <paramref name="period"/> with the regional settings adopted in IAD
        /// </summary>
        /// <param name="displayYear">Flag indicating whether to output the year in the string representation of the period</param>
        /// <returns>String representing the period in a display-friendly form </returns>
        public string ToDisplayName(bool displayYear = false)
        {
            if (period.IsEmpty)
            {
                return string.Empty;
            }

            if (period.EndDate == null)
            {
                return $"{period.StartDate:dd.MM.yyyy}-∞";
            }

            if (period.StartDate.Year != period.EndDateValue.Year)
            {
                return $"{period.StartDate:dd.MM.yyyy}-{period.EndDateValue:dd.MM.yyyy}";
            }

            if (period.StartDate.Month != period.EndDateValue.Month)
            {
                return period.ProcessYear($"{period.StartDate:dd.MM}-{period.EndDateValue:dd.MM}", displayYear);
            }

            var monthStr = period.StartDate.GetMonthStr();

            if (period.StartDate.IsFirstMonthDate() && period.EndDateValue.IsLastMonthDate())
            {
                return period.ProcessYear(monthStr, displayYear);
            }

            if (period.StartDate.Day == period.EndDateValue.Day)
            {
                return period.ProcessYear($"{period.StartDate:dd} {monthStr}", displayYear);
            }

            return period.ProcessYear($"{period.StartDate:dd}-{period.EndDateValue:dd} {monthStr}", displayYear);
        }

        /// <summary>
        /// Converts the period start date into a string representation of the month and year in the prepositional case for the Russian standard
        /// </summary>
        /// <returns>String containing the full month name and year in the prepositional case</returns>
        public string GetInMonthAndYearStrRus() => period.StartDate.GetInMonthAndYearStrRus();

        /// <summary>
        /// Converts the start and end dates of the period into a string description of the interval represented by months and year for the Russian standard
        /// </summary>
        /// <returns>String containing the month name and year if the period is within one month, otherwise the names of the start and end months with the year</returns>
        public string GetMonthAndYearStrRus() =>
            period.IsWithinOneMonth
                ? period.StartDate.GetMonthAndYearStrRus()
                : $"{period.StartDate.GetMonthAndYearStrRus()} - {period.EndDateValue.GetMonthAndYearStrRus()}";

        /// <summary>
        /// Converts the start and end dates of the period into a string description of the interval represented by months and year
        /// </summary>
        /// <returns>String containing the month name and year if the period is within one month, otherwise the names of the start and end months with the year</returns>
        public string GetMonthAndYearStr() =>
            period.IsWithinOneMonth
                ? period.StartDate.GetMonthAndYearStr()
                : $"{period.StartDate.GetMonthAndYearStr()} - {period.EndDateValue.GetMonthAndYearStr()}";

        /// <summary>
        /// Returns the week of the year to which the period start date belongs for the current standard
        /// </summary>
        /// <returns>A positive integer representing the week of the year to which the period start date belongs for the current standard</returns>
        public int GetWeekNumber() => period.GetWeekNumber(Thread.CurrentThread.CurrentCulture.Name);

        /// <summary>
        /// Returns the week of the year to which the period start date belongs for the <paramref name="isoCultureName"/> standard
        /// </summary>
        /// <param name="isoCultureName">Predefined CultureInfo name, name of an existing CultureInfo object, or a Windows-only culture name, case-insensitive</param>
        /// <returns>A positive integer representing the week of the year to which the period start date belongs for the specified standard</returns>
        public int GetWeekNumber(string isoCultureName)
        {
            var cultureInfo = new CultureInfo(isoCultureName);
            var calendar = cultureInfo.Calendar;
            var result = calendar.GetWeekOfYear(period.StartDate, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
            return result;
        }

        /// <summary>
        /// Converts the start and end dates of the period into a short string description of the interval represented by months and year
        /// </summary>
        /// <returns>String containing the month number and year if the period is within one month, otherwise the numbers of the start and end months with the year</returns>
        public string ToExtraShortString()
        {
            if (period.IsEmpty)
            {
                return string.Empty;
            }

            if (period.StartDate.Year != period.EndDateValue.Year)
            {
                return period.ToString();
            }

            var shortFormat = period.GetDayAndMonthDateFormat(Thread.CurrentThread.CurrentCulture);
            return $"{period.StartDate.ToString(shortFormat)}-{period.EndDateValue.ToString(shortFormat)}";
        }

        /// <summary>
        /// Converts the start and end dates of the period into a short string description of the interval represented by months and year for the <paramref name="cultureInfo"/> regional settings
        /// </summary>
        /// <param name="cultureInfo">Provides information about a specific language and regional settings used to format period dates</param>
        /// <returns>String containing the month number and year if the period is within one month, otherwise the numbers of the start and end months with the year</returns>
        public string ToExtraShortString(CultureInfo cultureInfo)
        {
            if (period.IsEmpty)
            {
                return string.Empty;
            }

            if (period.StartDate.Year != period.EndDateValue.Year)
            {
                return period.ToDisplayName(cultureInfo, true);
            }

            var shortFormat = period.GetDayAndMonthDateFormat(cultureInfo);

            return $"{period.StartDate.ToString(shortFormat, cultureInfo)}-{period.EndDateValue.ToString(shortFormat, cultureInfo)}";
        }

        /// <summary>
        /// Converts the start and end dates of the period into a string description of the interval represented by months and year for the Roman standard
        /// </summary>
        /// <returns>String containing the Roman month numeral and year if the period is within one month, otherwise the Roman numerals of the start and end months with the year</returns>
        public string GetMonthAndYearStrRoman() => MonthFormatter.GetMonthAndYearStrRoman(period);

        /// <summary>
        /// Returns the date format for the <paramref name="cultureInfo"/> regional settings containing only month and year
        /// </summary>
        /// <param name="cultureInfo">Provides information about a specific language and regional settings used to format period dates</param>
        /// <returns>String representing the date format in the specified regional settings for displaying only the month and year</returns>
        public string GetDayAndMonthDateFormat(CultureInfo cultureInfo)
        {
            var format = cultureInfo.DateTimeFormat;

            var shortFormat = format.ShortDatePattern;

            var dateSeparator = string.Empty;

            // remove Year pattern

            dateSeparator = format.DateSeparator;
            shortFormat = shortFormat
                          .Replace("Y", "y")
                          .Replace("y", string.Empty)
                          .Replace(dateSeparator + dateSeparator, dateSeparator);

            // remove DateSeparator from start
            if (shortFormat.StartsWith(dateSeparator))
            {
                shortFormat = shortFormat.Substring(dateSeparator.Length);
            }

            if (shortFormat.EndsWith(dateSeparator))
            {
                shortFormat = shortFormat.Substring(0, shortFormat.Length - dateSeparator.Length);
            }

            return shortFormat;
        }
    }

    public static bool Contains(this Period? period, DateTime? date)
        => period.HasValue && period.Value.StartDate <= date && (period.Value.EndDate == null || date <= period.Value.EndDateValue);

    private static string ProcessYear(this Period period, string result, bool addStartDateYear) => addStartDateYear ? $"{result} ({period.StartDate:yyyy})" : result;
}
