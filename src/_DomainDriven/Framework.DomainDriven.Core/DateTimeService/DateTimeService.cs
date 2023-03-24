using System;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven;

public class DateTimeService : IDateTimeService
{
    private readonly Func<DateTime> _getNow;

    private readonly Func<DateTime> _getToday;

    private readonly Func<DateTime> _getUtcNow;

    private readonly Func<Period> _getCurrentFinancialYear;

    private readonly Func<Period> _getCurrentMonth;

    private readonly Func<Period> _getCurrentYear;

    private readonly Func<Period> _getNextMonth;

    private readonly Func<Period> _getPrevMonth;


    public DateTimeService(
            [NotNull] Func<DateTime> getNow,
            [NotNull] Func<DateTime> getToday,
            [NotNull] Func<DateTime> getUtcNow,
            [NotNull] Func<Period> getCurrentFinancialYear,
            [NotNull] Func<Period> getCurrentMonth,
            [NotNull] Func<Period> getCurrentYear,
            [NotNull] Func<Period> getNextMonth,
            [NotNull] Func<Period> getPrevMonth)
    {
        this._getNow = getNow ?? throw new ArgumentNullException(nameof(getNow));
        this._getToday = getToday ?? throw new ArgumentNullException(nameof(getToday));
        this._getUtcNow = getUtcNow ?? throw new ArgumentNullException(nameof(getUtcNow));
        this._getCurrentFinancialYear = getCurrentFinancialYear ?? throw new ArgumentNullException(nameof(getCurrentFinancialYear));
        this._getCurrentMonth = getCurrentMonth ?? throw new ArgumentNullException(nameof(getCurrentMonth));
        this._getCurrentYear = getCurrentYear ?? throw new ArgumentNullException(nameof(getCurrentYear));
        this._getNextMonth = getNextMonth ?? throw new ArgumentNullException(nameof(getNextMonth));
        this._getPrevMonth = getPrevMonth ?? throw new ArgumentNullException(nameof(getPrevMonth));
    }


    public DateTime Now
    {
        get { return this._getNow(); }
    }

    public DateTime Today
    {
        get { return this._getToday(); }
    }

    public DateTime UtcNow
    {
        get { return this._getUtcNow(); }
    }

    public Period CurrentFinancialYear
    {
        get { return this._getCurrentFinancialYear(); }
    }

    public Period CurrentMonth
    {
        get { return this._getCurrentMonth(); }
    }

    public Period CurrentYear
    {
        get { return this._getCurrentYear(); }
    }

    public Period NextMonth
    {
        get { return this._getNextMonth(); }
    }

    public Period PrevMonth
    {
        get { return this._getPrevMonth(); }
    }


    public static readonly DateTimeService Default = new DateTimeService(() => DateTime.Now, () => DateTime.Today, () => DateTime.UtcNow, () => DateTime.Today.ToFinancialYearPeriod(), () => DateTime.Today.ToMonth(), () => DateTime.Today.ToYear(), () => DateTime.Today.AddMonth().ToMonth(), () => DateTime.Today.SubtractMonth().ToMonth());
}
