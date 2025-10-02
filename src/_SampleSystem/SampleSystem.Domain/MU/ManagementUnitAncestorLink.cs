using Framework.DomainDriven.Serialization;

namespace SampleSystem.Domain;

public class ManagementUnitAncestorLink : AuditPersistentDomainObjectBase
{
    private ManagementUnit ancestor;
    private ManagementUnit child;

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual ManagementUnit Ancestor
    {
        get { return this.ancestor; }
        set { this.ancestor = value; }
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual ManagementUnit Child
    {
        get { return this.child; }
        set { this.child = value; }
    }
}
