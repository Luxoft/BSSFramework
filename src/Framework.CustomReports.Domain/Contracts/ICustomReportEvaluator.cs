using Framework.Configuration.Domain.Models.Custom.Reports;

namespace Framework.CustomReports.Domain
{
    public interface ICustomReportEvaluator
    {
        IReportStream GetReportStream(ReportGenerationModel model);
    }
}
