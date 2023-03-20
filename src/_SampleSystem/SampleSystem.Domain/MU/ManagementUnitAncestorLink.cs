using System;

using Framework.DomainDriven.BLL;
using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace SampleSystem.Domain;

public class ManagementUnitAncestorLink :
        AuditPersistentDomainObjectBase,
        IModifiedHierarchicalAncestorLink<ManagementUnit, ManagementUnitToAncestorChildView, Guid>
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
