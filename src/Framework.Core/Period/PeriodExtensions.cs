// ReSharper disable once CheckNamespace
namespace Framework.Core;

/// <summary>
/// Extension methods for the period
/// </summary>
public static class PeriodExtensions
{
    /// <summary>
    /// Returns a value indicating whether the specified period <paramref name="period"/> intersects the <paramref name="target"/> period passed as a parameter
    /// </summary>
    /// <remarks>
    /// The comparison takes the sql database limit into account
    /// </remarks>
    /// <param name="period">Period in which the intersection is checked</param>
    /// <param name="target">Period to check the intersection with</param>
    /// <returns>true if the start or end date of the given period is within the specified period; otherwise false</returns>
    public static bool IsIntersected(this Period period, Period target) => !period.Intersect(target).IsEmpty;

    public static bool IsIntersected(this Period? period, Period? target) => period.HasValue && target.HasValue && period.Value.IsIntersected(target.Value);

    /// <summary>
    /// Returns a new period that is the sum of the given periods <paramref name="periods"/>
    /// </summary>
    /// <remarks>
    /// The comparison takes the sql database limit into account
    /// </remarks>
    /// <param name="periods">List of periods to sum</param>
    /// <returns>A new period whose start date is the minimum of the start dates of the given periods and whose end date is the maximum of the end dates of the given periods</returns>
    public static Period Aggregate(this IEnumerable<Period> periods)
    {
        if (periods is null)
        {
            throw new ArgumentNullException(nameof(periods));
        }

        return periods.Aggregate(Period.Empty, (p1, p2) => p1 + p2);
    }
}
