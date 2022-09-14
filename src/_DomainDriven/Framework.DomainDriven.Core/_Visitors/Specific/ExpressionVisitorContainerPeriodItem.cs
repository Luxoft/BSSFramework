using System;
using System.Collections.Generic;
using System.Linq.Expressions;

using Framework.Core;

namespace Framework.DomainDriven;

public class ExpressionVisitorContainerPeriodItem : IExpressionVisitorContainerItem
{
    public IEnumerable<ExpressionVisitor> GetVisitors()
    {
        yield return new OverrideMethodInfoVisitor<Func<Period, DateTime, bool>>(
         CommonPeriodExtensions.Contains,
         (period, date) => period.StartDate <= date && (period.EndDate == null || date <= period.EndDate));

        yield return new OverrideMethodInfoVisitor<Func<Period?, DateTime?, bool>>(
         CommonPeriodExtensions.Contains,
         (period, date) =>
                 period.HasValue
                 && date.HasValue
                 && (period.Value.StartDate <= date && (period.Value.EndDate == null || date <= period.Value.EndDate)));

        yield return new OverrideMethodInfoVisitor<Func<Period, DateTime?, bool>>(
         CommonPeriodExtensions.ContainsExt,
         (period, date) =>
                 period.StartDate <= date
                 && (period.EndDate == null || date <= period.EndDate));

        yield return new OverrideMethodInfoVisitor<Func<DateTime?, DateTime, bool>>(
         DateTimeExtensions.LessOrEqualIgnoreTime,
         (date, otherDate) => date < otherDate.Date.AddDays(1));

        yield return new OverrideMethodInfoVisitor<Func<Period, DateTime, bool>>(
         CommonPeriodExtensions.ContainsWithoutEndDate,
         (period, date) => period.StartDate <= date && (period.EndDate == null || date < period.EndDate));

        yield return new OverrideMethodInfoVisitor<Func<Period, Period, bool>>(
          CommonPeriodExtensions.Contains,
          (period, otherPeriod) =>

                  period.StartDate <= otherPeriod.StartDate // ContainsStartDate
              && (period.EndDate == null || otherPeriod.StartDate <= period.EndDate)
              && (period.StartDate <= otherPeriod.EndDate || otherPeriod.EndDate == null) // ContainsEndDate
              && (period.EndDate == null || (otherPeriod.EndDate != null && otherPeriod.EndDate <= period.EndDate)));

        yield return new OverrideMethodInfoVisitor<Func<Period, int?>>(
            CommonPeriodExtensions.TotalMonths,
                  period => !period.EndDate.HasValue || period.EndDate < period.StartDate ? (int?)null
                          : (period.EndDate.Value.Year - period.StartDate.Year) * 12 + period.EndDate.Value.Month - period.StartDate.Month);

        yield return new OverrideMethodInfoVisitor<Func<Period, Period, bool>>(
            PeriodExtensions.IsIntersected,
            (period, otherPeriod) => !((period.EndDate != null && period.EndDate < otherPeriod.StartDate)
                                    || (otherPeriod.EndDate != null && period.StartDate > otherPeriod.EndDate)));

        yield return new OverrideMethodInfoVisitor<Func<Period?, Period?, bool>>(
                PeriodExtensions.IsIntersected,
                (period, otherPeriod) => period.HasValue && otherPeriod.HasValue && (!((period.Value.EndDate != null && period.Value.EndDate < otherPeriod.Value.StartDate)
                                                                                    || (otherPeriod.Value.EndDate != null && period.Value.StartDate > otherPeriod.Value.EndDate))));

        yield return new OverrideMethodInfoVisitor(
            typeof(Period).GetEqualityMethod(),
            ExpressionHelper.Create((Period period, Period otherPeriod) =>
                period.StartDate == otherPeriod.StartDate && period.EndDate == otherPeriod.EndDate));

        yield return new OverrideMethodInfoVisitor(
            typeof(Period).GetInequalityMethod(),
            ExpressionHelper.Create((Period period, Period otherPeriod) =>
                period.StartDate != otherPeriod.StartDate || period.EndDate != otherPeriod.EndDate));
    }
}
