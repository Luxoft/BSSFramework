using System.IO;

namespace Framework.CustomReports.Domain
{
    public class ReportStream : IReportStream
    {
        public ReportStream(string name, ReportStreamType reportType, Stream stream)
        {
            this.Description = new ReportStreamDescription(name, reportType);
            this.Stream = stream;
        }

        public Stream Stream { get; }

        public IReportStreamDescription Description { get; }

        private class ReportStreamDescription : IReportStreamDescription
        {
            public ReportStreamDescription(string name, ReportStreamType reportType)
            {
                this.Name = name;
                this.ReportType = reportType;
            }

            public ReportStreamType ReportType { get; }

            public string Name { get; }
        }
    }
}