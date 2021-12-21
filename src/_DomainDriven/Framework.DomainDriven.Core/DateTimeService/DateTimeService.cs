using System;

using Framework.Core;

using JetBrains.Annotations;

namespace Framework.DomainDriven
{
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
            if (getNow == null) throw new ArgumentNullException(nameof(getNow));
            if (getToday == null) throw new ArgumentNullException(nameof(getToday));
            if (getUtcNow == null) throw new ArgumentNullException(nameof(getUtcNow));
            if (getCurrentFinancialYear == null) throw new ArgumentNullException(nameof(getCurrentFinancialYear));
            if (getCurrentMonth == null) throw new ArgumentNullException(nameof(getCurrentMonth));
            if (getCurrentYear == null) throw new ArgumentNullException(nameof(getCurrentYear));
            if (getNextMonth == null) throw new ArgumentNullException(nameof(getNextMonth));
            if (getPrevMonth == null) throw new ArgumentNullException(nameof(getPrevMonth));

            this._getNow = getNow;
            this._getToday = getToday;
            this._getUtcNow = getUtcNow;
            this._getCurrentFinancialYear = getCurrentFinancialYear;
            this._getCurrentMonth = getCurrentMonth;
            this._getCurrentYear = getCurrentYear;
            this._getNextMonth = getNextMonth;
            this._getPrevMonth = getPrevMonth;
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
}
