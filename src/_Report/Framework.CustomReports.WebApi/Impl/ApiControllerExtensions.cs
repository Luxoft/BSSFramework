using System;
using System.Net.Mime;

using Framework.CustomReports.Domain;
using Framework.DomainDriven;

using Microsoft.AspNetCore.Mvc;

namespace Framework.CustomReports.WebApi
{
    public static class ApiControllerExtensions
    {
        public static FileStreamResult GetReportResult(this ControllerBase controller, IReportStream result)
        {
            var (contentType, extension) = result.Description.ReportType switch
            {
                ReportStreamType.Excel => (ContentTypes.Excel, ReportExtenstions.Excel),
                ReportStreamType.Word => (ContentTypes.Word, ReportExtenstions.Word),
                ReportStreamType.Zip => (ContentTypes.Zip, ReportExtenstions.Zip),
                _ => throw new NotImplementedException($"reportType:'{result.Description.ReportType}' are not processed")
            };

            return controller.File(result.Stream, contentType, GetFileName(result.Description.Name, extension));
        }

        private static string GetFileName(string name, string extension)
        {
            var fileName = $"{name}_{DateTimeService.Default.Now:yyyy-MM-dd_HH-mm-ss-fff}.{extension}";
            return $"{fileName}";
        }

        internal static class ContentTypes
        {
            internal const string Excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            internal const string Word = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

            internal const string Zip = MediaTypeNames.Application.Zip;
        }

        public static class ReportExtenstions
        {
            public const string Excel = "xlsx";

            public const string Word = "docx";

            public const string Zip = "zip";
        }
    }
}
