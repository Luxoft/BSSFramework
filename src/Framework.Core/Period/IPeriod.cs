using System;
using System.Collections.Generic;

namespace Framework.Core
{
    public interface IPeriod
    {
        DateTime StartDate { get; set; }

        DateTime? EndDate { get; set; }

        DateTime EndDateValue { get; }

        DateTime NativeStartDate { get; }

        DateTime? NativeEndDate { get; }

        DateTime NativeEndDateValue { get; }

        bool IsWithinOneMonth { get; }

        bool IsEmpty { get; }

        TimeSpan Duration { get; }
    }

    public interface IPeriod<out T> : IPeriod
    {
        T RoundByMouths();

        T Round();

        IEnumerable<T> SplitToDays();

        IEnumerable<T> SplitToWeeks(DayOfWeek firstDay);

        IEnumerable<T> SplitToMonths();

        T NativeIntersect(IPeriod target);

        T Intersect(IPeriod otherPeriod);

        bool IsIntersectedExcludeZeroDuration(IPeriod target);

        bool IsNativeIntersected(IPeriod target);
    }
}
