using Framework.Core;

namespace Framework.Application.FinancialYear;

public interface IFinancialYearService
{
    Period GetCurrentFinancialYear();
}
