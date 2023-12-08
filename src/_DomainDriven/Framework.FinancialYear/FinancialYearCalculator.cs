using Framework.Core;

namespace Framework.FinancialYear;

public class FinancialYearCalculator (FinancialYearServiceSettings settings) : IFinancialYearCalculator
{
    private const int MinMonthValue = 1;

    private const int MaxMonthValue = 12;

    public Period GetFinancialYearPeriod(DateTime date)
    {
        var currentYearFinDate = new DateTime(date.Year, settings.StartMonth, 1);

        return date >= currentYearFinDate
                   ? new Period(currentYearFinDate, currentYearFinDate.AddYear().SubtractDay())
                   : new Period(currentYearFinDate.SubtractYear(), currentYearFinDate.SubtractDay());
    }

    public Period GetFinancialYearPeriod(int year)
    {
        return this.GetFinancialYearPeriod(new DateTime(year, settings.StartMonth, 1));
    }

    public int GetFinancialYear(int year, int month)
    {
        if (month < MinMonthValue || month > MaxMonthValue)
        {
            throw new ArgumentOutOfRangeException(nameof(month));
        }

        if (year < DateTime.MinValue.Year || year > DateTime.MaxValue.Year || (month >= settings.StartMonth && year == DateTime.MaxValue.Year))
        {
            throw new ArgumentOutOfRangeException(nameof(year));
        }

        return month >= settings.StartMonth ? year + 1 : year;
    }

    public bool IsStartFinancialYearDate(DateTime dateTime)
    {
        return dateTime.Day == 1 && dateTime.Month == settings.StartMonth;
    }
}
