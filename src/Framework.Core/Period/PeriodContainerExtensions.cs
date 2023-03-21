using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core;

public static class PeriodObjectExtensions
{
    public static void OverridePeriodStartDate (this IMutablePeriodObject source, DateTime startDate)
    {
        if (source == null)
        {
            throw new ArgumentNullException (nameof(source));
        }


        source.Period = new Period(startDate, source.Period.EndDate);
    }

    public static void OverridePeriodEndDate (this IMutablePeriodObject source, DateTime? endDate)
    {
        if (source == null)
        {
            throw new ArgumentNullException (nameof(source));
        }


        source.Period = new Period (source.Period.StartDate, endDate);
    }

    public static T GetNewest<T> (this IEnumerable<T> source)
            where T : IPeriodObject
    {
        var request = from item in source
                      orderby item.Period descending
                      select item;

        return request.FirstOrDefault ();
    }

    public static T GetForDate<T>(this IEnumerable<T> source, DateTime date)
            where T : IPeriodObject
    {
        var request = from item in source
                      where item.Period.Contains(date)
                      select item;

        return request.SingleOrDefault();
    }

    public static IEnumerable<T> OrderByPeriod<T> (this IEnumerable<T> source)
            where T : IPeriodObject
    {
        var request = from item in source
                      orderby item.Period
                      select item;

        return request;
    }

    public static IEnumerable<Period> GetPeriods<TItem>(this IEnumerable<TItem> source)
            where TItem : IPeriodObject
    {
        return from item in source
               select item.Period into period
               orderby period
               select period;
    }
}
