using Framework.Core;
using Framework.DomainDriven.BLL;

namespace SampleSystem.Domain.IntegrationVersions;

[BLLIntegrationSaveRole(CountType = CountType.Both)]
[BLLIntegrationRemoveRole]
public class IntegrationVersionContainer1 : ExternalDomainObject
{
    private string name;

    public virtual string Name
    {
        get => this.name;
        set => this.name = value;
    }
}
