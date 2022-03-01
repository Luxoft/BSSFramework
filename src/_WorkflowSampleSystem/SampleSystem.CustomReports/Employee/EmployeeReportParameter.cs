using System;

using Framework.CustomReports.Attributes;
using Framework.Restriction;

using SampleSystem.Domain;

namespace SampleSystem.CustomReports.Employee
{
    public class EmployeeReportParameter : ReportParameterBase
    {
        [Required]
        public Guid ReportId { get; set; }

        [ReportParameterVisualIdentity("EnglishName")]
        public virtual EmployeePosition Position { get; set; }
    }
}
