namespace Framework.Core;

/// <summary>
/// Методы расширения для периода
/// </summary>
public static class PeriodExtensions
{
    /// <summary>
    /// Возвращает значение, указывающее, пересекает ли указанный период <see cref="period"/> значение периода <see cref="target"/> переданного в качестве параметра
    /// </summary>
    /// <remarks>
    /// Сравнение происходит с учетом ограничения sql базы данных
    /// </remarks>
    /// <param name="period">Период, в котором проверяется пересечение</param>
    /// <param name="target">Период, с которым проверяется пересечение</param>
    /// <returns>true, если дата начала или дата окончания переданного периода содержатся в указанном периоде, в противном случае — false</returns>
    public static bool IsIntersected(this Period period, Period target)
    {
        return !period.Intersect(target).IsEmpty;
    }

    public static bool IsIntersected(this Period? period, Period? target)
    {
        return period.HasValue && target.HasValue && period.Value.IsIntersected(target.Value);
    }

    /// <summary>
    /// Возвращает новый период являющийся суммой переданных периодов <see cref="periods"/>
    /// </summary>
    /// <remarks>
    /// Сравнение происходит с учетом ограничения sql базы данных
    /// </remarks>
    /// <param name="periods">Список периодов, которые необходимо просуммировать</param>
    /// <returns>Новый период, дата старта которого является минимальной из дат старта переданных периодов, а дата окончания является максимальной из дат окончания переденных периодов</returns>
    public static Period Aggregate(this IEnumerable<Period> periods)
    {
        if (periods == null)
        {
            throw new ArgumentNullException(nameof(periods));
        }

        return periods.Aggregate(Period.Empty, (p1, p2) => p1 + p2);
    }



    /// <summary>
    /// Используя текущий период возвращает новый период, который начинается с первого дня начального месяца периода и заканчивается последним днем конечного месяца периода
    /// </summary>
    /// <returns>Новый период, который начинается и заканчивается в первом и последнем дне граничных месяцев</returns>
    public static Period RoundByMouths(this Period period)
    {
        return new Period(period.StartDate.ToStartMonthDate(), period.EndDate.MaybeNullableToNullable(d => d.ToEndMonthDate()));
    }

    /// <summary>
    /// Используя текущий период возвращает новый период, дата начала и конца (при возможности) которого имеет значение времени 00:00:00
    /// </summary>
    /// <returns></returns>
    public static Period Round(this Period period)
    {
        return new Period(period.StartDate.Date, period.EndDate.MaybeNullableToNullable(d => d.Date));
    }

    /// <summary>
    /// Для всех дней содержащихся в периоде создается новый период длинною в один этот день
    /// </summary>
    /// <returns>Перечисление всех периодов длинною в один день, которые находятся в переданном временном интервале</returns>
    public static IEnumerable<Period> SplitToDays(this Period period)
    {
        for (var date = period.StartDate.Date; date <= period.EndDateValue; date = date.AddDay())
        {
            yield return new Period(date, date);
        }
    }

    /// <summary>
    /// Для всех недель содержащихся в периоде создается новый период длинною в одину эту неделю, последняя неделя может быть не полной
    /// </summary>
    /// <returns>Перечисление всех периодов длинною в одину неделю, которые находятся в переданном временном интервале</returns>
    public static IEnumerable<Period> SplitToWeeks(this Period period, DayOfWeek firstDay = DayOfWeek.Monday)
    {
        return period.SplitToWeeksInternal(firstDay).OrderBy(v => v);
    }

    /// <summary>
    /// Для всех месяцев содержащихся в периоде создается новый период длинною в одинин этот месяц, последний месяц может быть не полном
    /// </summary>
    /// <returns>Перечисление всех периодов длинною в один месяц, которые находятся в переданном временном интервале</returns>
    public static IEnumerable<Period> SplitToMonths(this Period period)
    {
        if (period.IsEmpty)
        {
            yield break;
        }

        if (!period.StartDate.IsFirstMonthDate())
        {
            if (period.StartDate.ToEndMonthDate() < period.EndDateValue)
            {
                yield return new Period(period.StartDate, period.StartDate.ToEndMonthDate());
            }
            else
            {
                yield return new Period(period.StartDate, period.EndDateValue);
                yield break;
            }
        }

        for (var currentDate = period.StartDate.IsFirstMonthDate() ? period.StartDate : period.StartDate.ToStartMonthDate().AddMonths(1);
             currentDate.ToEndMonthDate() <= period.EndDateValue;
             currentDate = currentDate.AddMonths(1))
        {
            yield return new Period(currentDate, currentDate.ToEndMonthDate());
        }

        if (!period.EndDateValue.IsLastMonthDate())
        {
            yield return new Period(period.EndDateValue.ToStartMonthDate(), period.EndDateValue);
        }
    }

    /// <summary>
    /// Возвращает новый период, который является пересечением текущего периода с переданным в качестве параметра периода <see cref="otherPeriod"/>
    /// </summary>
    /// <remarks>
    /// Сравнение происходит с учетом ограничения sql базы данных
    /// </remarks>
    /// <param name="otherPeriod">Период, из которого извлекаются даты, входящие также в указанный период</param>
    /// <returns>Новый период представляющий собой пересечение дат заданных двух периодов. Если пересечение дат нету, то период будет пустым</returns>
    public static Period Intersect(this Period period, Period otherPeriod)
    {
        return new Period(period.StartDate.Max(otherPeriod.StartDate), period.EndDate.UnsafeOperation(otherPeriod.EndDate, (d1, d2) => d1.Min(d2)));
    }

    /// <summary>
    /// Возвращает значение, указывающее, пересекает ли текущий период значение периода <see cref="target"/> переданного в качестве параметра
    /// Если полученный при вычислении период имеет нулевую продолжительность, то читается, что периоды не пересекаются
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public static bool IsIntersectedExcludeZeroDuration(this Period period, Period target)
    {
        var intersect = period.Intersect(target);

        if (intersect.IsEmpty)
        {
            return false;
        }

        return intersect.Duration != TimeSpan.Zero;
    }

    private static IEnumerable<Period> SplitToWeeksInternal(this Period period, DayOfWeek firstDay = DayOfWeek.Monday)
    {
        var weekStartDate = period.StartDate.Date;

        var weekEndDate =
            weekStartDate.AddDays(
                (14 - (int)weekStartDate.DayOfWeek - (int)DayOfWeek.Monday + (int)firstDay) % 7);

        while (weekEndDate < period.EndDate)
        {
            yield return new Period(weekStartDate, weekEndDate);
            weekStartDate = weekEndDate.AddDay();
            weekEndDate = weekEndDate.AddWeek();
        }

        if (weekStartDate <= period.EndDate)
        {
            yield return new Period(weekStartDate, period.EndDate);
        }
    }
}
