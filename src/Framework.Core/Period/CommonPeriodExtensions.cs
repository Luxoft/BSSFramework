using System.Globalization;

using CommonFramework;

// ReSharper disable once CheckNamespace
namespace Framework.Core;

/// <summary>
/// Методы расширения для типов реализующих интерфейс Period
/// </summary>
public static class CommonPeriodExtensions
{
    /// <param name="period">Период, в котором проверяется пересечение</param>
    extension(Period period)
    {
        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение периода <see cref="target"/> переданного в качестве параметра
        /// </summary>
        /// <remarks>
        /// Сравнение происходит с учетом ограничения sql базы данных
        /// </remarks>
        /// <param name="target">Период, с которым проверяется пересечение</param>
        /// <returns>true, если дата начала и дата окончания переданного периода содержатся в указанном периоде, в противном случае — false</returns>
        public bool Contains(Period target)
            =>
                period.Contains(target.StartDate) && period.Contains(target.EndDateValue);

        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение даты <see cref="date"/> переданной в качестве параметра
        /// </summary>
        /// <remarks>
        /// Сравнение происходит с учетом ограничения sql базы данных
        /// </remarks>
        /// <param name="date">Дата, которую нужно проверить на вхождение в период</param>
        /// <returns>true, если дата содержится в периоде, в противном случае — false</returns>
        public bool Contains(DateTime date)
            => period.StartDate <= date && (period.EndDate == null || date <= period.EndDateValue);
    }

    /// <param name="period">Период, в котором проверяется дата</param>
    extension(Period period)
    {
        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение даты <see cref="date"/> переданной в качестве параметра
        /// </summary>
        /// <remarks>
        /// Сравнение происходит с учетом ограничения sql базы данных
        /// </remarks>
        /// <param name="date">Дата, которую нужно проверить на вхождение в период</param>
        /// <returns>true, если дата не является null и дата содержится в периоде, в противном случае — false</returns>
        public bool ContainsExt(DateTime? date)
            => date.MaybeNullable(v => period.Contains(v));

        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение даты <see cref="date"/> переданной в качестве параметра
        /// </summary>
        /// <param name="date">Дата, которую нужно проверить на вхождение в период</param>
        /// <returns>true, если дата содержится в периоде, в противном случае — false</returns>
        public bool NativeContains(DateTime date) => period.NativeStartDate <= date && (period.NativeEndDate == null || date <= period.NativeEndDateValue);

        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение даты <see cref="date"/> переданной в качестве параметра
        /// </summary>
        /// <param name="date">Дата, которую нужно проверить на вхождение в период</param>
        /// <returns>true, если дата не является null и дата содержится в периоде, в противном случае — false</returns>
        public bool NativeContains(DateTime? date) => date.MaybeNullable(z => period.NativeContains(z));

        /// <summary>
        /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/>, без учета даты окончания периода, значение даты <see cref="date"/> переданной в качестве параметра
        /// </summary>
        /// <remarks>
        /// Сравнение происходит с учетом ограничения sql базы данных
        /// </remarks>
        /// <param name="date">Дата, которую нужно проверить на вхождение в период</param>
        /// <returns>true, если дата содержится в периоде и не равна дате окончания периода, в противном случае — false</returns>
        public bool ContainsWithoutEndDate(DateTime date) => period.StartDate <= date && (period.EndDate == null || date < period.EndDateValue);

        /// <summary>
        /// Возвращает список дат в периоде, выравненный по рабочей части недели
        /// (т.е. для недели возвращаются даты, соответсвуещие периоду с понедельника по пятницу)
        /// </summary>
        public IEnumerable<DateTime?> DatesByWorkingWeek()
        {
            if (period.EndDate == null)
            {
                throw new ArgumentException("нельзя выполнить операцию для данного объекта");
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
        /// <returns>Если EndDate равен null или EndDate больше StartDate, то возвращает null, иначе количество месяцев</returns>
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
        /// Возвращает строковое представление текущего периода <see cref="period"/> для региональных настроек <see cref="cultureInfo"/>
        /// </summary>
        /// <param name="cultureInfo">Региональные настройки, в которых необходимо отобразить период</param>
        /// <param name="displayYear">Флаг, указывающий выводить ли строковом представлении периода год</param>
        /// <returns>Строка, представляющая период в удобной для отображения форме</returns>
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
        /// Возвращает строковое представление текущего периода <see cref="period"/> с региональными настройками принятыми в IAD
        /// </summary>
        /// <param name="displayYear">Флаг, указывающий выводить ли строковом представлении периода год</param>
        /// <returns>Строка, представляющая период в удобной для отображения форме </returns>
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
        /// Преобразует дату начала периода в строковое представление месяца и года в предложном падеже для Российского стандарта
        /// </summary>
        /// <returns>Строка, содержащая полное имя месяца и год в предложном падеже</returns>
        public string GetInMonthAndYearStrRus() => period.StartDate.GetInMonthAndYearStrRus();

        /// <summary>
        /// Преобразует дату начала и конца периода в строковое описание интервала представленый месяцами и годом для Российского стандарта
        /// </summary>
        /// <returns>Строка, содержащая содержащая имя месяца и год если период в одном месяце, иначе содержащяя имя начального и конечного месяца с годом</returns>
        public string GetMonthAndYearStrRus() =>
            period.IsWithinOneMonth
                ? period.StartDate.GetMonthAndYearStrRus()
                : $"{period.StartDate.GetMonthAndYearStrRus()} - {period.EndDateValue.GetMonthAndYearStrRus()}";

        /// <summary>
        /// Преобразует дату начала и конца периода в строковое описание интервала представленый месяцами и годом
        /// </summary>
        /// <returns>Строка, содержащая содержащая имя месяца и год если период в одном месяце, иначе содержащяя имя начального и конечного месяца с годом</returns>
        public string GetMonthAndYearStr() =>
            period.IsWithinOneMonth
                ? period.StartDate.GetMonthAndYearStr()
                : $"{period.StartDate.GetMonthAndYearStr()} - {period.EndDateValue.GetMonthAndYearStr()}";

        /// <summary>
        /// Возвращает неделю года, к которой относится дата начала периода для текущего стандарта
        /// </summary>
        /// <returns>Положительное целое число, представляющее неделю года, к которой относится дата начала периода для текущего стандарта</returns>
        public int GetWeekNumber() => period.GetWeekNumber(Thread.CurrentThread.CurrentCulture.Name);

        /// <summary>
        /// Возвращает неделю года, к которой относится дата начала периода для <see cref="isoCultureName"/> стандарта
        /// </summary>
        /// <param name="isoCultureName">Предварительно определенное имя CultureInfo, существующего объекта CultureInfo или имя языка и региональных параметров, свойственных только Windows, не учитывая регистр</param>
        /// <returns>Положительное целое число, представляющее неделю года, к которой относится дата начала периода для указанного стандарта</returns>
        public int GetWeekNumber(string isoCultureName)
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
        /// Преобразует дату начала и конца периода в короткое строковое описание интервала представленый месяцами и годом для <see cref="cultureInfo"/> региональных параметров
        /// </summary>
        /// <param name="cultureInfo">Предоставляет сведения об определенном языке и региональных параметрах используемое для форматирование дат периода</param>
        /// <returns>Строка, содержащая содержащая номер месяца и год если период в одном месяце, иначе содержащяя намера начального и конечного месяца с годом</returns>
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
        /// Преобразует дату начала и конца периода в строковое описание интервала представленый месяцами и годом для Римского стандарта
        /// </summary>
        /// <returns>Строка, содержащая содержащая римский номер месяца и год если период в одном месяце, иначе содержащяя римские номера начального и конечного месяца с годом</returns>
        public string GetMonthAndYearStrRoman() => MonthFormatter.GetMonthAndYearStrRoman(period);

        /// <summary>
        /// Возвращает формат даты для <see cref="cultureInfo"/> региональных параметров содержащий только месяц и год
        /// </summary>
        /// <param name="cultureInfo">Предоставляет сведения об определенном языке и региональных параметрах используемое для форматирование дат периода</param>
        /// <returns>Строка, представляющая формат даты, в указанных региональных параметрах, для вывода только месяца и года</returns>
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
