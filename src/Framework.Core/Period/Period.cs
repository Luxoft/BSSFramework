using System.Runtime.Serialization;

using Anch.Core;

// ReSharper disable once CheckNamespace
namespace Framework.Core;

[DataContract(Namespace = "")]
public partial struct Period : IEquatable<Period>, IComparable<Period>, IComparable
{
    private DateTime startDate;

    private DateTime? endDate;

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

    public Period(DateTime startDate, DateTime? endDate = null)
            : this()
    {
        this.StartDate = startDate;
        this.EndDate = endDate;
    }

    public Period(int year, int month)
            : this()
    {
        this.StartDate = new DateTime(year, month, 1);
        this.EndDate = this.startDate.ToEndMonthDate();
    }

    public Period(string startDate, string? endDate = null)
            : this()
    {
        this.StartDate = DateTime.Parse(startDate);
        this.EndDate = endDate.MaybeToNullable(DateTime.Parse);
    }

    [IgnoreDataMember]
    public TimeSpan Duration => this.EndDateValue - this.StartDate;

    [IgnoreDataMember]
    public bool IsEmpty => this.StartDate > this.EndDateValue;

    [IgnoreDataMember]
    public bool IsWithinOneMonth => this.StartDate.Month == this.EndDateValue.Month && this.StartDate.Year == this.EndDateValue.Year;

    [IgnoreDataMember]
    public bool IsMonth => this.StartDate.ToStartMonthDate() == this.StartDate && this.EndDateValue == this.StartDate.ToEndMonthDate();

    #region Public.Method

    public bool Equals(Period other) => this.StartDate == other.StartDate && this.EndDate == other.EndDate;

    public int CompareTo(object? other) => this.CompareTo((Period)other!);

    public int CompareTo(Period other)
    {
        var startDateCompare = this.StartDate.CompareTo(other.startDate);

        return startDateCompare == 0 ? this.EndDateValue.CompareTo(other.EndDateValue) : startDateCompare;
    }

    public override bool Equals(object? obj) => obj is Period && this.Equals((Period)obj);

    public override int GetHashCode() => this.StartDate.GetHashCode() ^ this.EndDate.GetHashCode();

    public override string ToString() => this.ToDisplayName(true);

    /// <summary>
    /// Using the current period, returns a new period that starts on the first day of the period's start month and ends on the last day of the period's end month
    /// </summary>
    /// <returns>A new period that starts and ends on the first and last days of the boundary months</returns>
    public Period RoundByMouths() => new(this.StartDate.ToStartMonthDate(), this.EndDate.MaybeNullableToNullable(d => d.ToEndMonthDate()));

    /// <summary>
    /// Using the current period, returns a new period whose start and end dates (where possible) have the time value 00:00:00
    /// </summary>
    /// <returns></returns>
    public Period Round() => new(this.StartDate.Date, this.EndDate.MaybeNullableToNullable(d => d.Date));

    /// <summary>
    /// For every day in the period a new one-day period is created
    /// </summary>
    /// <returns>Enumeration of all one-day periods within the given time interval</returns>
    public IEnumerable<Period> SplitToDays()
    {
        for (var date = this.StartDate.Date; date <= this.EndDateValue; date = date.AddDay())
        {
            yield return new Period(date, date);
        }
    }

    /// <summary>
    /// For every week in the period a new one-week period is created; the last week may be incomplete
    /// </summary>
    /// <returns>Enumeration of all one-week periods within the given time interval</returns>
    public IEnumerable<Period> SplitToWeeks(DayOfWeek firstDay = DayOfWeek.Monday) => this.SplitToWeeksInternal(firstDay).OrderBy(v => v);

    /// <summary>
    /// For every month in the period a new one-month period is created; the last month may be incomplete
    /// </summary>
    /// <returns>Enumeration of all one-month periods within the given time interval</returns>
    public IEnumerable<Period> SplitToMonths()
    {
        if (this.IsEmpty)
        {
            yield break;
        }

        if (!this.StartDate.IsFirstMonthDate())
        {
            if (this.StartDate.ToEndMonthDate() < this.EndDateValue)
            {
                yield return new Period(this.StartDate, this.StartDate.ToEndMonthDate());
            }
            else
            {
                yield return new Period(this.StartDate, this.EndDateValue);
                yield break;
            }
        }

        for (var currentDate = this.StartDate.IsFirstMonthDate() ? this.StartDate : this.StartDate.ToStartMonthDate().AddMonths(1);
             currentDate.ToEndMonthDate() <= this.EndDateValue;
             currentDate = currentDate.AddMonths(1))
        {
            yield return new Period(currentDate, currentDate.ToEndMonthDate());
        }

        if (!this.EndDateValue.IsLastMonthDate())
        {
            yield return new Period(this.EndDateValue.ToStartMonthDate(), this.EndDateValue);
        }
    }

    /// <summary>
    /// Returns a new period that is the intersection of the current period with the period <paramref name="otherPeriod"/> passed as a parameter
    /// </summary>
    /// <remarks>
    /// The comparison takes the sql database limit into account
    /// </remarks>
    /// <param name="otherPeriod">Period from which dates that are also within the specified period are extracted</param>
    /// <returns>A new period that is the intersection of the dates of the two given periods. If the dates do not intersect, the period will be empty</returns>
    public Period Intersect(Period otherPeriod) => new(this.StartDate.Max(otherPeriod.StartDate), this.EndDate.UnsafeOperation(otherPeriod.EndDate, (d1, d2) => d1.Min(d2)));

    /// <summary>
    /// Returns a new period that is the intersection of the current period with the period <paramref name="target"/> passed as a parameter
    /// </summary>
    /// <param name="target">Period from which dates that are also within the specified period are extracted</param>
    /// <returns>A new period that is the intersection of the dates of the two given periods. If the dates do not intersect, the period will be empty</returns>
    public Period NativeIntersect(Period target)
    {
        var result = new Period(
                                target.NativeStartDate > this.NativeStartDate ? target.NativeStartDate : this.NativeStartDate,
                                target.NativeEndDate < this.NativeEndDateValue ? target.NativeEndDateValue : this.NativeEndDateValue);

        return result.IsEmpty ? Empty : result;
    }

    /// <summary>
    /// Returns a value indicating whether the current period intersects the <paramref name="target"/> period passed as a parameter
    /// If the period obtained by the calculation has zero duration, the periods are considered not intersecting
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsIntersectedExcludeZeroDuration(Period target)
    {
        var intersect = this.Intersect(target);

        if (intersect.IsEmpty)
        {
            return false;
        }

        return intersect.Duration != TimeSpan.Zero;
    }

    /// <summary>
    /// Returns a value indicating whether the current period intersects the <paramref name="target"/> period passed as a parameter
    /// </summary>
    /// <param name="target">Period to check the intersection with</param>
    /// <returns>true if the start or end date of the given period is within the specified period; otherwise false</returns>
    public bool IsNativeIntersected(Period target)
    {
        var nativeIntersectedResult = this.NativeIntersect(target);
        var empty = Empty;
        return nativeIntersectedResult.NativeStartDate != empty.NativeStartDate || nativeIntersectedResult.NativeEndDateValue != empty.NativeEndDateValue;
    }

    #endregion

    #region Public.Static.Method

    /// <summary>
    /// Parse string
    /// Ex:
    /// 2014-01-01@2014-05-04
    /// 2014-01-01
    /// Pass NullOrWhiteSpace - result <see cref="Period.Empty"/>
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    public static Period Parse(string periodAsString)
    {
        if (string.IsNullOrWhiteSpace(periodAsString))
        {
            return Empty;
        }

        if (!periodAsString.Contains("@"))
        {
            return DateTime.Parse(periodAsString).ToPeriod();
        }


        var dates = periodAsString.Split('@');
        if (dates.Length > 2)
        {
            throw new ArgumentException($"Incorrect period string: '{periodAsString}'");
        }

        var startDate = DateTime.Parse(dates[0]);
        var endDate = DateTime.Parse(dates[1]);
        return new Period(startDate, endDate);
    }

    public static Period operator +(Period p1, Period p2) => new(p1.StartDate.Min<DateTime>(p2.StartDate), p1.EndDateValue.Max<DateTime>(p2.EndDateValue));

    public static bool operator ==(Period p1, Period p2) => p1.Equals(p2);

    public static bool operator !=(Period p1, Period p2) => !p1.Equals(p2);

    public static Period FromYear(int year)
    {
        var start = new DateTime(year, 1, 1);

        return start.ToPeriod(start.ToEndYearDate());
    }

    #endregion

    #region Private methods

    private IEnumerable<Period> SplitToWeeksInternal(DayOfWeek firstDay = DayOfWeek.Monday)
    {
        var weekStartDate = this.StartDate.Date;

        var weekEndDate =
                weekStartDate.AddDays(
                                      (14 - (int)weekStartDate.DayOfWeek - (int)DayOfWeek.Monday + (int)firstDay) % 7);

        while (weekEndDate < this.EndDate)
        {
            yield return new Period(weekStartDate, weekEndDate);
            weekStartDate = weekEndDate.AddDay();
            weekEndDate = weekEndDate.AddWeek();
        }

        if (weekStartDate <= this.EndDate)
        {
            yield return new Period(weekStartDate, this.EndDate);
        }
    }

    #endregion
}

/// <summary>
/// Time interval expressed by two dates
/// </summary>
[Serializable]
public partial struct Period : IDeserializationCallback
{
    #region Interface

    void IDeserializationCallback.OnDeserialization(object? _)
    {
        this.startDate = this.startDate.ToSqlDateTime();
        this.endDate = this.endDate.ToSqlDateTime();
    }

    #endregion

    #region Public.Property

    /// <summary>
    /// Period start date taking into account sql database limits
    /// </summary>
    [DataMember]
    public DateTime StartDate
    {
        get => this.startDate.ToSqlDateTime();
        set => this.startDate = value.ToSqlDateTime();
    }

    /// <summary>
    /// Period end date taking into account sql database limits
    /// </summary>
    [DataMember]
    public DateTime? EndDate
    {
        get => this.endDate.ToSqlDateTime();
        set => this.endDate = value.ToSqlDateTime();
    }

    /// <summary>
    /// If <see cref="EndDate"/> is not set, returns the maximum date, otherwise the period end date taking into account sql database limits
    /// </summary>
    [IgnoreDataMember]
    public DateTime EndDateValue => this.endDate is null ? Eternity.EndDateValue : this.endDate.Value.ToSqlDateTime();

    /// <summary>
    /// Period start date
    /// </summary>
    [IgnoreDataMember]
    public DateTime NativeStartDate => this.startDate;

    /// <summary>
    /// Period end date
    /// </summary>
    [IgnoreDataMember]
    public DateTime? NativeEndDate => this.endDate;

    /// <summary>
    /// If <see cref="EndDate"/> is not set, returns the maximum date, otherwise the period end date
    /// </summary>
    [IgnoreDataMember]
    public DateTime NativeEndDateValue => this.endDate is null ? Eternity.NativeEndDateValue : this.endDate.Value;

    #endregion

    #region Public.Static.Property

    /// <summary>
    /// Provides an instance of an infinite period
    /// </summary>
    public static readonly Period Eternity = new(DateTime.MinValue.ToSqlDateTime(),
                                                 DateTime.MaxValue.ToSqlDateTime());

    /// <summary>
    /// Provides an instance of an empty period
    /// </summary>
    public static readonly Period Empty = new(DateTime.MaxValue.ToSqlDateTime(),
                                              DateTime.MinValue.ToSqlDateTime());

    #endregion
}
