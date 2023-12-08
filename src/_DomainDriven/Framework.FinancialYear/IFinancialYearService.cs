using Framework.Core;

namespace Framework.FinancialYear;

public interface IFinancialYearService
{
    Period GetCurrentFinancialYear();
}
