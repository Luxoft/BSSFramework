using Framework.Core;

namespace Framework.FinancialYear;

public class FinancialYearService(TimeProvider timeProvider, IFinancialYearCalculator financialYearCalculator) : IFinancialYearService
{
    public Period GetCurrentFinancialYear()
    {
        return financialYearCalculator.GetFinancialYearPeriod(timeProvider.GetLocalNow().DateTime);
    }
}
