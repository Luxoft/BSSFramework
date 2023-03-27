using Framework.DomainDriven.Serialization;
using Framework.Persistent;

namespace SampleSystem.Domain;

public class LocationAncestorLink : AuditPersistentDomainObjectBase,
                                    IModifiedHierarchicalAncestorLink<Location, LocationToAncestorChildView, Guid>
{
    private Location ancestor;

    private Location child;

    public LocationAncestorLink()
    {
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual Location Ancestor
    {
        get { return this.ancestor; }
        set { this.ancestor = value; }
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual Location Child
    {
        get { return this.child; }
        set { this.child = value; }
    }
}
