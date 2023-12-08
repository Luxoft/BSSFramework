using Framework.Core;

namespace Framework.FinancialYear;

public interface IFinancialYearCalculator
{
    int GetFinancialYear(int year, int month = 1);

    Period GetFinancialYearPeriod(DateTime date);

    Period GetFinancialYearPeriod(int year);

    Period ToNextFinancialYearPeriod(DateTime date) => this.GetFinancialYearPeriod(date.AddYear());

    bool IsFinancialYear(Period period) => this.GetFinancialYearPeriod(period.StartDate) == period;

    bool IsStartFinancialYearDate(DateTime dateTime);
}
