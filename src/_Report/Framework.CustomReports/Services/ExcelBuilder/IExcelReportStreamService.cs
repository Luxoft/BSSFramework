using System.Collections.Generic;
using System.IO;

namespace Framework.CustomReports.Services.ExcelBuilder
{
    public interface IExcelReportStreamService
    {
        Stream Generate<TAnon>(
                   string reportName,
                   ExcelHeaderDesign<TAnon>[] headers,
                   IList<TAnon> anonValues,
                   EvaluateParameterInfo parameterModel);
    }
}
