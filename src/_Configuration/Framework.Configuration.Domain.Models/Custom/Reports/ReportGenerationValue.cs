using Framework.Configuration.Domain.Reports;

namespace Framework.Configuration.Domain.Models.Custom.Reports
{
    public class ReportGenerationValue : DomainObjectBase
    {
        public ReportParameter Parameter { get; set; }

        public string Value { get; set; }

        public string DesignValue { get; set; }
    }
}