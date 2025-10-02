using Framework.DomainDriven.Serialization;

namespace SampleSystem.Domain;

public class LocationAncestorLink : AuditPersistentDomainObjectBase
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
