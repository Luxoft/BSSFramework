using System.Collections.Generic;

using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.CustomReports.Domain;
using Framework.OData;

namespace Framework.CustomReports.Services
{
    public interface ISelectOperationByReportParser
    {
        SelectOperation Parse(ReportGenerationModel model);
    }
}