using Framework.Configuration.Domain.Reports;
using Framework.DomainDriven;
using Framework.DomainDriven.SerializeMetadata;

namespace Framework.Configuration.Domain.Models.Custom.Reports;

[DirectMode(DirectMode.Out)]
public class ReportGenerationRequestModel : DomainObjectBase
{
    public List<ReportParameter> Parameters { get; set; }

    public List<TypeMetadata> TypeMetadatas { get; set; }
}
