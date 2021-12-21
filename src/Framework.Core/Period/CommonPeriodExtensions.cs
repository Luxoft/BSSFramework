using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Framework.Core
{
    /// <summary>
    /// Методы расширения для типов реализующих интерфейс IPeriod
    /// </summary>
    public static class CommonPeriodExtensions
    {
        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение периода <see cref="target"/> переданного в качестве параметра
        /// </summary>
        /// <remarks>
        /// Сравнение происходит с учетом ограничения sql базы данных
        /// </remarks>
        /// <param name="period">Период, в котором проверяется пересечение</param>
        /// <param name="target">Период, с которым проверяется пересечение</param>
        /// <returns>true, если дата начала и дата окончания переданного периода содержатся в указанном периоде, в противном случае — false</returns>
        public static bool Contains<T>(this T period, T target)
            where T : struct, IPeriod
        {
            return period.Contains(target.StartDate) && period.Contains(target.EndDateValue);
        }

        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение даты <see cref="date"/> переданной в качестве параметра
        /// </summary>
        /// <remarks>
        /// Сравнение происходит с учетом ограничения sql базы данных
        /// </remarks>
        /// <param name="period">Период, в котором проверяется дата</param>
        /// <param name="date">Дата, которую нужно проверить на вхождение в период</param>
        /// <returns>true, если дата содержится в периоде, в противном случае — false</returns>
        public static bool Contains<T>(this T period, DateTime date)
            where T : struct, IPeriod
        {
            return period.StartDate <= date && (period.EndDate == null || date <= period.EndDateValue);
        }

        public static bool Contains<T>(this T? period, DateTime? date)
            where T : struct, IPeriod
        {
            return period.HasValue && period.Value.StartDate <= date && (period.Value.EndDate == null || date <= period.Value.EndDateValue);
        }

        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение даты <see cref="date"/> переданной в качестве параметра
        /// </summary>
        /// <remarks>
        /// Сравнение происходит с учетом ограничения sql базы данных
        /// </remarks>
        /// <param name="period">Период, в котором проверяется дата</param>
        /// <param name="date">Дата, которую нужно проверить на вхождение в период</param>
        /// <returns>true, если дата не является null и дата содержится в периоде, в противном случае — false</returns>
        public static bool ContainsExt<T>(this T period, DateTime? date)
            where T : struct, IPeriod
        {
            return date.MaybeNullable(v => period.Contains(v));
        }

        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение даты <see cref="date"/> переданной в качестве параметра
        /// </summary>
        /// <param name="period">Период, в котором проверяется дата</param>
        /// <param name="date">Дата, которую нужно проверить на вхождение в период</param>
        /// <returns>true, если дата содержится в периоде, в противном случае — false</returns>
        public static bool NativeContains<T>(this T period, DateTime date) where T : IPeriod
        {
            return period.NativeStartDate <= date && (period.NativeEndDate == null || date <= period.NativeEndDateValue);
        }

        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение даты <see cref="date"/> переданной в качестве параметра
        /// </summary>
        /// <param name="period">Период, в котором проверяется дата</param>
        /// <param name="date">Дата, которую нужно проверить на вхождение в период</param>
        /// <returns>true, если дата не является null и дата содержится в периоде, в противном случае — false</returns>
        public static bool NativeContains<T>(this T period, DateTime? date) where T : IPeriod
        {
            return date.MaybeNullable(z => period.NativeContains(z));
        }

        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/>, без учета даты окончания периода, значение даты <see cref="date"/> переданной в качестве параметра
        /// </summary>
        /// <remarks>
        /// Сравнение происходит с учетом ограничения sql базы данных
        /// </remarks>
        /// <param name="period">Период, в котором проверяется дата</param>
        /// <param name="date">Дата, которую нужно проверить на вхождение в период</param>
        /// <returns>true, если дата содержится в периоде и не равна дате окончания периода, в противном случае — false</returns>
        public static bool ContainsWithoutEndDate<T>(this T period, DateTime date) where T : IPeriod
        {
            return period.StartDate <= date && (period.EndDate == null || date < period.EndDateValue);
        }

        /// <summary>
        /// Возвращает список дат в периоде, выравненный по рабочей части недели
        /// (т.е. для недели возвращаются даты, соответсвуещие периоду с понедельника по пятницу)
        /// </summary>
        public static IEnumerable<DateTime?> DatesByWorkingWeek<T>(this T period) where T : IPeriod
        {
            if (period.EndDate == null)
            {
                throw new System.ArgumentException("нельзя выполнить операцию для данного объекта");
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
        /// Возвращает количество целых месяцев в периоде
        /// </summary>
        /// <param name="period">Период, по которому считаются месяцы</param>
        /// <returns>Если EndDate равен null или EndDate больше StartDate, то возвращает null, иначе количество месяцев</returns>
        public static int? TotalMonths<T>(this T period) where T : IPeriod
        {
            if (!period.EndDate.HasValue || period.EndDate < period.StartDate)
            {
                return null;
            }

            return (period.EndDate.Value.Year - period.StartDate.Year) * 12
                   + period.EndDate.Value.Month - period.StartDate.Month;
        }

        /// <summary>
        /// Возвращает строковое представление текущего периода <see cref="period"/> для региональных настроек <see cref="cultureInfo"/>
        /// </summary>
        /// <param name="period">Период, который необходимо отобразить в строковое представление</param>
        /// <param name="cultureInfo">Региональные настройки, в которых необходимо отобразить период</param>
        /// <param name="displayYear">Флаг, указывающий выводить ли строковом представлении периода год</param>
        /// <returns>Строка, представляющая период в удобной для отображения форме</returns>
        public static string ToDisplayName<T>(this T period, CultureInfo cultureInfo, bool displayYear = false) where T : IPeriod
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
                return $"{period.StartDate.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo)}-{period.EndDateValue.ToString(cultureInfo.DateTimeFormat.ShortDatePattern, cultureInfo)}";
            }

            if (period.StartDate.Month != period.EndDateValue.Month)
            {
                var shortFormat = period.GetDayAndMonthDateFormat(cultureInfo);

                return period.ProressYear($"{period.StartDate.ToString(shortFormat, cultureInfo)}-{period.EndDateValue.ToString(shortFormat, cultureInfo)}", displayYear);
            }

            var monthStr = cultureInfo.DateTimeFormat.GetMonthName(period.StartDate.Month);

            if (period.StartDate.IsFirstMonthDate() && period.EndDateValue.IsLastMonthDate())
            {
                return period.ProressYear(monthStr, displayYear);
            }

            if (period.StartDate.Day == period.EndDateValue.Day)
            {
                return period.ProressYear($"{period.StartDate:dd} {monthStr}", displayYear);
            }

            return period.ProressYear($"{period.StartDate:dd}-{period.EndDateValue:dd} {monthStr}", displayYear);
        }

        /// <summary>
        /// Возвращает строковое представление текущего периода <see cref="period"/> с региональными настройками принятыми в IAD
        /// </summary>
        /// <param name="period">Период, который необходимо отобразить в строковое представление</param>
        /// <param name="displayYear">Флаг, указывающий выводить ли строковом представлении периода год</param>
        /// <returns>Строка, представляющая период в удобной для отображения форме </returns>
        public static string ToDisplayName<T>(this T period, bool displayYear = false) where T : IPeriod
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
                return period.ProressYear($"{period.StartDate:dd.MM}-{period.EndDateValue:dd.MM}", displayYear);
            }

            var monthStr = period.StartDate.GetMonthStr();

            if (period.StartDate.IsFirstMonthDate() && period.EndDateValue.IsLastMonthDate())
            {
                return period.ProressYear(monthStr, displayYear);
            }

            if (period.StartDate.Day == period.EndDateValue.Day)
            {
                return period.ProressYear($"{period.StartDate:dd} {monthStr}", displayYear);
            }

            return period.ProressYear($"{period.StartDate:dd}-{period.EndDateValue:dd} {monthStr}", displayYear);
        }

        #region Working with period in reports

        /// <summary>
        /// Преобразует дату начала периода в строковое представление месяца и года в предложном падеже для Российского стандарта
        /// </summary>
        /// <returns>Строка, содержащая полное имя месяца и год в предложном падеже</returns>
        public static string GetInMonthAndYearStrRus<T>(this T period) where T : IPeriod
        {
            return period.StartDate.GetInMonthAndYearStrRus();
        }

        /// <summary>
        /// Преобразует дату начала и конца периода в строковое описание интервала представленый месяцами и годом для Российского стандарта
        /// </summary>
        /// <returns>Строка, содержащая содержащая имя месяца и год если период в одном месяце, иначе содержащяя имя начального и конечного месяца с годом</returns>
        public static string GetMonthAndYearStrRus<T>(this T period) where T : IPeriod
        {
            return period.IsWithinOneMonth
                ? period.StartDate.GetMonthAndYearStrRus()
                : $"{period.StartDate.GetMonthAndYearStrRus()} - {period.EndDateValue.GetMonthAndYearStrRus()}";
        }

        /// <summary>
        /// Преобразует дату начала и конца периода в строковое описание интервала представленый месяцами и годом
        /// </summary>
        /// <returns>Строка, содержащая содержащая имя месяца и год если период в одном месяце, иначе содержащяя имя начального и конечного месяца с годом</returns>
        public static string GetMonthAndYearStr<T>(this T period) where T : IPeriod
        {
            return period.IsWithinOneMonth
                ? period.StartDate.GetMonthAndYearStr()
                : $"{period.StartDate.GetMonthAndYearStr()} - {period.EndDateValue.GetMonthAndYearStr()}";
        }

        /// <summary>
        /// Возвращает неделю года, к которой относится дата начала периода для текущего стандарта
        /// </summary>
        /// <returns>Положительное целое число, представляющее неделю года, к которой относится дата начала периода для текущего стандарта</returns>
        public static int GetWeekNumber<T>(this T period) where T : IPeriod
        {
            return period.GetWeekNumber(Thread.CurrentThread.CurrentCulture.Name);
        }

        /// <summary>
        /// Возвращает неделю года, к которой относится дата начала периода для <see cref="isoCultureName"/> стандарта
        /// </summary>
        /// <param name="period"></param>
        /// <param name="isoCultureName">Предварительно определенное имя CultureInfo, существующего объекта CultureInfo или имя языка и региональных параметров, свойственных только Windows, не учитывая регистр</param>
        /// <returns>Положительное целое число, представляющее неделю года, к которой относится дата начала периода для указанного стандарта</returns>
        public static int GetWeekNumber<T>(this T period, string isoCultureName) where T : IPeriod
        {
            var cultureInfo = new CultureInfo(isoCultureName);
            var calendar = cultureInfo.Calendar;
            var result = calendar.GetWeekOfYear(period.StartDate, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
            return result;
        }

        /// <summary>
        /// Преобразует дату начала и конца периода в короткое строковое описание интервала представленый месяцами и годом
        /// </summary>
        /// <returns>Строка, содержащая содержащая номер месяца и год если период в одном месяце, иначе содержащяя намера начального и конечного месяца с годом</returns>
        public static string ToExtraShortString<T>(this T period) where T : IPeriod
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
        /// Преобразует дату начала и конца периода в короткое строковое описание интервала представленый месяцами и годом для <see cref="cultureinfo"/> региональных параметров
        /// </summary>
        /// <param name="cultureinfo">Предоставляет сведения об определенном языке и региональных параметрах используемое для форматирование дат периода</param>
        /// <returns>Строка, содержащая содержащая номер месяца и год если период в одном месяце, иначе содержащяя намера начального и конечного месяца с годом</returns>
        public static string ToExtraShortString<T>(this T period, CultureInfo cultureinfo) where T : IPeriod
        {
            if (period.IsEmpty)
            {
                return string.Empty;
            }

            if (period.StartDate.Year != period.EndDateValue.Year)
            {
                return period.ToDisplayName(cultureinfo, true);
            }

            var shortFormat = period.GetDayAndMonthDateFormat(cultureinfo);

            return $"{period.StartDate.ToString(shortFormat, cultureinfo)}-{period.EndDateValue.ToString(shortFormat, cultureinfo)}";
        }

        /// <summary>
        /// Преобразует дату начала и конца периода в строковое описание интервала представленый месяцами и годом для Римского стандарта
        /// </summary>
        /// <returns>Строка, содержащая содержащая римский номер месяца и год если период в одном месяце, иначе содержащяя римские номера начального и конечного месяца с годом</returns>
        public static string GetMonthAndYearStrRoman<T>(this T period) where T : IPeriod
        {
            return MonthFormatter.GetMonthAndYearStrRoman(period);
        }

        /// <summary>
        /// Возвращает формат даты для <see cref="cultureinfo"/> региональных параметров содержащий только месяц и год
        /// </summary>
        /// <param name="cultureinfo">Предоставляет сведения об определенном языке и региональных параметрах используемое для форматирование дат периода</param>
        /// <returns>Строка, представляющая формат даты, в указанных региональных параметрах, для вывода только месяца и года</returns>
        public static string GetDayAndMonthDateFormat<T>(this T period, CultureInfo cultureinfo) where T : IPeriod
        {
            var format = cultureinfo.DateTimeFormat;

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

        #endregion

        private static string ProressYear<T>(this T period, string result, bool addStartDateYear) where T : IPeriod
        {
            return addStartDateYear ? $"{result} ({period.StartDate:yyyy})" : result;
        }
    }
}
