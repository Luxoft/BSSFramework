using Framework.BLL.Domain.Serialization;

namespace SampleSystem.Domain.MU;

public class ManagementUnitAncestorLink : AuditPersistentDomainObjectBase
{
    private ManagementUnit ancestor = null!;
    private ManagementUnit child = null!;

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual ManagementUnit Ancestor
    {
        get => this.ancestor;
        set => this.ancestor = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual ManagementUnit Child
    {
        get => this.child;
        set => this.child = value;
    }
}

