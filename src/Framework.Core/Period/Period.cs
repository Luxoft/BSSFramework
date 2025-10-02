using System.Runtime.Serialization;

using CommonFramework;

namespace Framework.Core;

[DataContract(Namespace = "")]
public partial struct Period : IEquatable<Period>, IComparable<Period>, IComparable, IPeriod<Period>
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

    public Period(string startDate, string endDate = null)
            : this()
    {
        this.StartDate = DateTime.Parse(startDate);
        this.EndDate = endDate.MaybeToNullable(DateTime.Parse);
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

    #region Public.Method

    public bool Equals(Period other)
    {
        return this.StartDate == other.StartDate && this.EndDate == other.EndDate;
    }

    public int CompareTo(object other)
    {
        return this.CompareTo((Period)other);
    }

    public int CompareTo(Period other)
    {
        var startDateCompare = this.StartDate.CompareTo(other.startDate);

        return startDateCompare == 0 ? this.EndDateValue.CompareTo(other.EndDateValue) : startDateCompare;
    }

    public override bool Equals(object obj)
    {
        return obj is Period && this.Equals((Period)obj);
    }

    public override int GetHashCode()
    {
        return this.StartDate.GetHashCode() ^ this.EndDate.GetHashCode();
    }

    public override string ToString()
    {
        return this.ToDisplayName(true);
    }

    /// <summary>
    /// Используя текущий период возвращает новый период, который начинается с первого дня начального месяца периода и заканчивается последним днем конечного месяца периода
    /// </summary>
    /// <returns>Новый период, который начинается и заканчивается в первом и последнем дне граничных месяцев</returns>
    public Period RoundByMouths()
    {
        return new Period(this.StartDate.ToStartMonthDate(), this.EndDate.MaybeNullableToNullable(d => d.ToEndMonthDate()));
    }

    /// <summary>
    /// Используя текущий период возвращает новый период, дата начала и конца (при возможности) которого имеет значение времени 00:00:00
    /// </summary>
    /// <returns></returns>
    public Period Round()
    {
        return new Period(this.StartDate.Date, this.EndDate.MaybeNullableToNullable(d => d.Date));
    }

    /// <summary>
    /// Для всех дней содержащихся в периоде создается новый период длинною в один этот день
    /// </summary>
    /// <returns>Перечисление всех периодов длинною в один день, которые находятся в переданном временном интервале</returns>
    public IEnumerable<Period> SplitToDays()
    {
        for (var date = this.StartDate.Date; date <= this.EndDateValue; date = date.AddDay())
        {
            yield return new Period(date, date);
        }
    }

    /// <summary>
    /// Для всех недель содержащихся в периоде создается новый период длинною в одину эту неделю, последняя неделя может быть не полной
    /// </summary>
    /// <returns>Перечисление всех периодов длинною в одину неделю, которые находятся в переданном временном интервале</returns>
    public IEnumerable<Period> SplitToWeeks(DayOfWeek firstDay = DayOfWeek.Monday)
    {
        return this.SplitToWeeksInternal(firstDay).OrderBy(v => v);
    }

    /// <summary>
    /// Для всех месяцев содержащихся в периоде создается новый период длинною в одинин этот месяц, последний месяц может быть не полном
    /// </summary>
    /// <returns>Перечисление всех периодов длинною в один месяц, которые находятся в переданном временном интервале</returns>
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
    /// Возвращает новый период, который является пересечением текущего периода с переданным в качестве параметра периода <see cref="otherPeriod"/>
    /// </summary>
    /// <remarks>
    /// Сравнение происходит с учетом ограничения sql базы данных
    /// </remarks>
    /// <param name="otherPeriod">Период, из которого извлекаются даты, входящие также в указанный период</param>
    /// <returns>Новый период представляющий собой пересечение дат заданных двух периодов. Если пересечение дат нету, то период будет пустым</returns>
    public Period Intersect(IPeriod otherPeriod)
    {
        return new Period(this.StartDate.Max(otherPeriod.StartDate), this.EndDate.UnsafeOperation(otherPeriod.EndDate, (d1, d2) => d1.Min(d2)));
    }

    /// <summary>
    /// Возвращает новый период, который является пересечением текущего периода с переданным в качестве параметра периода <see cref="target"/>
    /// </summary>
    /// <param name="target">Период, из которого извлекаются даты, входящие также в указанный период</param>
    /// <returns>Новый период представляющий собой пересечение дат заданных двух периодов. Если пересечение дат нету, то период будет пустым</returns>
    public Period NativeIntersect(IPeriod target)
    {
        var result = new Period(
                                target.NativeStartDate > this.NativeStartDate ? target.NativeStartDate : this.NativeStartDate,
                                target.NativeEndDate < this.NativeEndDateValue ? target.NativeEndDateValue : this.NativeEndDateValue);

        return result.IsEmpty ? Empty : result;
    }

    /// <summary>
    /// Возвращает значение, указывающее, пересекает ли текущий период значение периода <see cref="target"/> переданного в качестве параметра
    /// Если полученный при вычислении период имеет нулевую продолжительность, то читается, что периоды не пересекаются
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool IsIntersectedExcludeZeroDuration(IPeriod target)
    {
        var intersect = this.Intersect(target);

        if (intersect.IsEmpty)
        {
            return false;
        }

        return intersect.Duration != TimeSpan.Zero;
    }

    /// <summary>
    /// Возвращает значение, указывающее, пересекает ли текущий период значение периода <see cref="target"/> переданного в качестве параметра
    /// </summary>
    /// <param name="target">Период, с которым проверяется пересечение</param>
    /// <returns>true, если дата начала или дата окончания переданного периода содержатся в указанном периоде, в противном случае — false</returns>
    public bool IsNativeIntersected(IPeriod target)
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

    public static Period operator +(Period p1, Period p2)
    {
        return new Period(p1.StartDate.Min(p2.StartDate), p1.EndDateValue.Max(p2.EndDateValue));
    }

    public static bool operator ==(Period p1, Period p2)
    {
        return p1.Equals(p2);
    }

    public static bool operator !=(Period p1, Period p2)
    {
        return !p1.Equals(p2);
    }


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
/// Интервал времени выраженный двумя датами
/// </summary>
[Serializable]
public partial struct Period : IDeserializationCallback
{
    #region Interface

    void IDeserializationCallback.OnDeserialization(object _)
    {
        this.startDate = this.startDate.ToSqlDateTime();
        this.endDate = this.endDate.ToSqlDateTime();
    }

    #endregion

    #region Public.Property

    /// <summary>
    /// Дата начала периода с учетом ограничей sql базы данных
    /// </summary>
    [DataMember]
    public DateTime StartDate
    {
        get { return this.startDate.ToSqlDateTime(); }
        set { this.startDate = value.ToSqlDateTime(); }
    }

    /// <summary>
    /// Дата окончания периода с учетом ограничей sql базы данных
    /// </summary>
    [DataMember]
    public DateTime? EndDate
    {
        get { return this.endDate.ToSqlDateTime(); }
        set { this.endDate = value.ToSqlDateTime(); }
    }

    /// <summary>
    /// Если <see cref="EndDate"/> не задана, то возвращает максимальную дату, иначе дату окончания периода с учетом ограничей sql базы данных
    /// </summary>
    [IgnoreDataMember]
    public DateTime EndDateValue
    {
        get { return this.endDate == null ? Eternity.EndDateValue : this.endDate.Value.ToSqlDateTime(); }
    }

    /// <summary>
    /// Дата начала периода
    /// </summary>
    [IgnoreDataMember]
    public DateTime NativeStartDate
    {
        get { return this.startDate; }
    }

    /// <summary>
    /// Дата окончания периода
    /// </summary>
    [IgnoreDataMember]
    public DateTime? NativeEndDate
    {
        get { return this.endDate; }
    }

    /// <summary>
    /// Если <see cref="EndDate"/> не задана, то возвращает максимальную дату, иначе дату окончания периода
    /// </summary>
    [IgnoreDataMember]
    public DateTime NativeEndDateValue
    {
        get { return this.endDate == null ? Eternity.NativeEndDateValue : this.endDate.Value; }
    }


    #endregion

    #region Public.Static.Property

    /// <summary>
    /// Предоставляет экземпляр бесконечного периода
    /// </summary>
    public static readonly Period Eternity = new Period(DateTime.MinValue.ToSqlDateTime(),
                                                        DateTime.MaxValue.ToSqlDateTime());

    /// <summary>
    /// Предоставляет экземпляр пустого периода
    /// </summary>
    public static readonly Period Empty = new Period(DateTime.MaxValue.ToSqlDateTime(),
                                                     DateTime.MinValue.ToSqlDateTime());

    #endregion
}
