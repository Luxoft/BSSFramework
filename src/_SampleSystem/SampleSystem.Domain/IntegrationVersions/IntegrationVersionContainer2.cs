using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Serialization;
using Framework.BLL.Domain.ServiceRole;
using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace SampleSystem.Domain.IntegrationVersions;

[BLLIntegrationSaveRole]
public class IntegrationVersionContainer2 : ExternalDomainObject
{
    private string name;

    public virtual string Name
    {
        get => this.name;
        set => this.name = value;
    }

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.All ^ DTORole.Integration)]
    [IntegrationVersion(IntegrationPolicy = ApplyIntegrationPolicy.IgnoreLessVersion)]
    public override long IntegrationVersion
    {
        get => base.IntegrationVersion;
        set => base.IntegrationVersion = value;
    }
}
