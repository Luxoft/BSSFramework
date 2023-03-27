using Framework.Core;
using Framework.DomainDriven.BLL;
using Framework.Persistent;

namespace SampleSystem.Domain.IntergrationVersions;

[DomainType("D1972415-C65B-42D7-ADBB-561B03935E70")]
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
