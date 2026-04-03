using Framework.BLL.Domain.Attributes;
using Framework.BLL.Domain.Serialization;

namespace SampleSystem.Domain.IntegrationVersions;

public abstract class ExternalDomainObject : AuditPersistentDomainObjectBase
{
    private long integrationVersion;

    [CustomSerialization(CustomSerializationMode.Ignore, DTORole.All ^ DTORole.Integration)]
    [IntegrationVersion(IntegrationPolicy = ApplyIntegrationPolicy.IgnoreLessOrEqualVersion)]
    public virtual long IntegrationVersion
    {
        get => this.integrationVersion;
        set => this.integrationVersion = value;
    }
}
