using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace SampleSystem.Domain.IntergrationVersions;

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
