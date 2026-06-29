using Framework.BLL.Domain.Serialization;

namespace SampleSystem.Domain.Directories;

public class LocationAncestorLink : AuditPersistentDomainObjectBase
{
    private Location ancestor = null!;

    private Location child = null!;

    public LocationAncestorLink()
    {
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual Location Ancestor
    {
        get => this.ancestor;
        set => this.ancestor = value;
    }

    [CustomSerialization(CustomSerializationMode.ReadOnly)]
    public virtual Location Child
    {
        get => this.child;
        set => this.child = value;
    }
}

