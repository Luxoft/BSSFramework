using Framework.Configuration.Domain.Reports;
using Framework.DomainDriven.Serialization;
using Framework.Restriction;

namespace Framework.Configuration.Domain.Models.Custom.Reports;

public class ReportParameterValue : DomainObjectBase
{
    [Required]
    public ReportParameter ReportParameter { get; set; }

    [Required]
    public string Value { get; set; }

    [Required]
    public string DesignValue { get; set; }

    [CustomSerialization(CustomSerializationMode.Ignore)]
    public Guid StrictId { get; set; }
}
