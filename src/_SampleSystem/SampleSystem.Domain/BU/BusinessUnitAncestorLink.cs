using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace SampleSystem.Domain;

public class BusinessUnitAncestorLink : AuditPersistentDomainObjectBase,
                                        IModifiedHierarchicalAncestorLink<BusinessUnit, BusinessUnitToAncestorChildView, Guid>
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
