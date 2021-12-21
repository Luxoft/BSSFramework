namespace Framework.CustomReports.Domain
{
    public interface IReportStreamDescription
    {
        ReportStreamType ReportType { get; }

        string Name { get; }
    }
}