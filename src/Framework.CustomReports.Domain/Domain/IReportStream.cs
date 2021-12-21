using System.IO;

namespace Framework.CustomReports.Domain
{
    public interface IReportStream
    {
        Stream Stream { get; }

        IReportStreamDescription Description { get; }
    }
}