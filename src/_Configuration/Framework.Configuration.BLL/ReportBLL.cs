using System.Linq;
using Framework.Core;
using Framework.Validation;

namespace Framework.Configuration.BLL;

public partial class ReportBLL
{
    public override void Save(Domain.Reports.Report report)
    {
        var parameterNames = report.Parameters.Select(z => z.Name).ToHashSet();

        var filterByParameters = report.Filters.Where(z => z.IsValueFromParameters);

        var emptyParameterNames = filterByParameters.Where(z => !parameterNames.Contains(z.Value)).ToList();

        if (emptyParameterNames.Any())
        {
            throw new ValidationException("Filter refer to absent parameters ('{0}')", emptyParameterNames.Join(","));
        }

        // elegant solution for fix report(aggregate) modification in persistent layer.
        report.ModifyDate = null;

        base.Save(report);
    }
}
