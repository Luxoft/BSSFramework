using Framework.BLL.Domain.ServiceRole;

using SampleSystem.Domain.IntegrationVersions;
using SampleSystem.Domain.Models.Integration._Base;

namespace SampleSystem.Domain.Models.Integration;

[BLLIntegrationSaveRole(AllowCreate = true)]
public class IntegrationVersionContainer1CustomIntegrationSaveModel : DomainObjectIntegrationSaveModel<IntegrationVersionContainer1>
{
    public string CustomName
    {
        get;
        set;
    }
}
