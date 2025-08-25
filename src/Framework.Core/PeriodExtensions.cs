using CommonFramework;

namespace Framework.Core;

public static class PeriodHelper
{
    public static readonly Period Eternity = new Period(DateTime.MinValue, DateTime.MaxValue);

    /// <summary>
    /// Предоставляет экземпляр пустого периода
    /// </summary>
    public static readonly Period Empty = new Period(DateTime.MaxValue, DateTime.MinValue);
}

public static class PeriodExtensions
{
    public static bool IsEmpty(this Period period)
    {
        return period == PeriodHelper.Empty;
    }

    public static DateTime GetEndDateValue(this Period period)
    {
        return period.EndDate ?? PeriodHelper.Eternity.GetEndDateValue();
    }

    /// <summary>
    /// Возвращает значение, указывающее, содержит ли указанный период <see cref="period"/> значение периода <see cref="target"/> переданного в качестве параметра
    /// </summary>
    /// <remarks>
    /// Сравнение происходит с учетом ограничения sql базы данных
    /// </remarks>
    /// <param name="period">Период, в котором проверяется пересечение</param>
    /// <param name="target">Период, с которым проверяется пересечение</param>
    /// <returns>true, если дата начала и дата окончания переданного периода содержатся в указанном периоде, в противном случае — false</returns>
    public static bool Contains(this Period period, Period target)
    {
        return period.Contains(target.StartDate) && period.Contains(target.GetEndDateValue());
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
    public static bool Contains(this Period period, DateTime date)
    {
        return period.StartDate <= date && (period.EndDate == null || date <= period.GetEndDateValue());
    }

    public static bool Contains(this Period? period, DateTime? date)
    {
        return period.HasValue && period.Value.StartDate <= date && (period.Value.EndDate == null || date <= period.Value.GetEndDateValue());
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
    public static bool ContainsExt(this Period period, DateTime? date)
    {
        return date.MaybeNullable(v => period.Contains(v));
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
    public static bool ContainsWithoutEndDate(this Period period, DateTime date)
    {
        return period.StartDate <= date && (period.EndDate == null || date < period.GetEndDateValue());
    }
}
