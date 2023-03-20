using System;
using System.Collections.Generic;
using System.Linq;

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
}
