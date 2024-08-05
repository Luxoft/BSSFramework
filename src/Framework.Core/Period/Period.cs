using System.Numerics;
using System.Runtime.Serialization;

namespace Framework.Core;

[DataContract(Namespace = "")]
public record struct Period(DateTime StartDate, DateTime? EndDate = null) : IAdditionOperators<Period, Period, Period>, IParsable<Period>
{
    public Period(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay)
            : this(new DateTime(startYear, startMonth, startDay), new DateTime(endYear, endMonth, endDay))
    {
    }

    public Period(int startYear, int startMonth, int startDay)
            : this(new DateTime(startYear, startMonth, startDay))
    {
    }

    public Period(int startYear, int startMonth, int startDay, int endMonth, int endDay)
            : this(new DateTime(startYear, startMonth, startDay), new DateTime(startYear, endMonth, endDay))
    {
    }

    public Period(int startMonth, int startDay, int endMonth, int endDay)
            : this(new DateTime(DateTime.Today.Year, startMonth, startDay), new DateTime(DateTime.Today.Year, endMonth, endDay))
    {
    }

    public Period(int year, int month)
            : this(new DateTime(year, month, 1), new DateTime(year, month, 1).ToEndMonthDate())
    {
    }

    /// <summary>
    /// Если <see cref="EndDate"/> не задана, то возвращает максимальную дату, иначе дату окончания периода с учетом ограничей sql базы данных
    /// </summary>
    [IgnoreDataMember]
    public DateTime EndDateValue
    {
        get { return this.EndDate ?? Eternity.EndDateValue; }
    }

    [IgnoreDataMember]
    public TimeSpan Duration
    {
        get { return this.EndDateValue - this.StartDate; }
    }

    [IgnoreDataMember]
    public bool IsEmpty
    {
        get { return this.StartDate > this.EndDateValue; }
    }

    [IgnoreDataMember]
    public bool IsWithinOneMonth
    {
        get { return this.StartDate.Month == this.EndDateValue.Month && this.StartDate.Year == this.EndDateValue.Year; }
    }

    [IgnoreDataMember]
    public bool IsMonth
    {
        get { return this.StartDate.ToStartMonthDate() == this.StartDate && this.EndDateValue == this.StartDate.ToEndMonthDate(); }
    }

    public override string ToString()
    {
        return this.ToDisplayName(true);
    }

    /// <summary>
    /// Parse string
    /// Ex:
    /// 2014-01-01@2014-05-04
    /// 2014-01-01
    /// Pass NullOrWhiteSpace - result <see cref="Period.Empty"/>
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public static Period Parse(string periodAsString, IFormatProvider provider = null)
    {
        return TryParse(periodAsString, provider).GetValue();
    }

    public static bool TryParse(string periodAsString, IFormatProvider provider, out Period result)
    {
        if (TryParse(periodAsString, provider) is ISuccessResult<Period> successResult)
        {
            result = successResult.Result;
            return true;
        }
        else
        {
            result = Period.Empty;
            return false;
        }
    }

    private static ITryResult<Period> TryParse(string periodAsString, IFormatProvider provider)
    {
        if (string.IsNullOrWhiteSpace(periodAsString))
        {
            return TryResult.Return(Empty);
        }

        if (!periodAsString.Contains("@"))
        {
            return TryResult.Return(DateTime.Parse(periodAsString, provider).ToPeriod());
        }

        var dates = periodAsString.Split('@');
        if (dates.Length > 2)
        {
            return TryResult.CreateFault<Period>(new ArgumentException($"Incorrect period string: '{periodAsString}'"));
        }

        var startDate = DateTime.Parse(dates[0], provider);

        var endDate = DateTime.Parse(dates[1], provider);

        return TryResult.Return(new Period(startDate, endDate));
    }

    public static Period operator +(Period p1, Period p2)
    {
        return new Period(p1.StartDate.Min(p2.StartDate), p1.EndDateValue.Max(p2.EndDateValue));
    }

    public static Period FromYear(int year)
    {
        var start = new DateTime(year, 1, 1);

        return start.ToPeriod(start.ToEndYearDate());
    }

    /// <summary>
    /// Предоставляет экземпляр бесконечного периода
    /// </summary>
    public static readonly Period Eternity = new (DateTime.MinValue, DateTime.MaxValue);

    /// <summary>
    /// Предоставляет экземпляр пустого периода
    /// </summary>
    public static readonly Period Empty = new (DateTime.MaxValue, DateTime.MinValue);
}
