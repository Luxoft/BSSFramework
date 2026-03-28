using Framework.Core;


namespace Framework.Application.FinancialYear;

public class FinancialYearService(TimeProvider timeProvider, IFinancialYearCalculator financialYearCalculator) : IFinancialYearService
{
    public Period GetCurrentFinancialYear() => financialYearCalculator.GetFinancialYearPeriod(timeProvider.GetLocalNow().DateTime);
}
