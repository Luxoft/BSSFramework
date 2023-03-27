using Framework.Configuration.Domain.Reports;
using Framework.DomainDriven;
using Framework.DomainDriven.Serialization;

namespace Framework.Configuration.Domain.Models.Custom.Reports;

[DirectMode(DirectMode.In)]
public class ReportGenerationModel : DomainObjectBase
{
    public Report Report { get; set; }

    public IEnumerable<ReportGenerationValue> Items { get; set; }


    [CustomSerialization(CustomSerializationMode.Ignore)]
    public IEnumerable<ReportGenerationPredefineValue> PredefineGenerationValues { get; set; }
}
