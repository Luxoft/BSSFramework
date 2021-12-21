using Framework.Configuration.Domain.Models.Custom.Reports;
using Framework.CustomReports.Domain;

namespace Framework.CustomReports.Services
{
    public interface IReportStreamService
    {
        IReportStream GetReportByModel(ReportGenerationModel model);
    }
}