using Framework.DomainDriven.Serialization;

namespace SampleSystem.Domain;

public class BusinessUnitAncestorLink : AuditPersistentDomainObjectBase
{
    private BusinessUnit ancestor;

    private BusinessUnit child;

    public BusinessUnitAncestorLink()
    {
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual BusinessUnit Ancestor
    {
        get { return this.ancestor; }
        set { this.ancestor = value; }
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual BusinessUnit Child
    {
        get { return this.child; }
        set { this.child = value; }
    }
}
