using Framework.Persistent.Mapping;

namespace SampleSystem.Domain;

[View]
public class LocationToAncestorChildView : AuditPersistentDomainObjectBase
{
    private Location source;

    private Location childOrAncestor;

    public virtual Location Source
    {
        get { return this.source; }
        set { this.source = value; }
    }

    public virtual Location ChildOrAncestor
    {
        get { return this.childOrAncestor; }
        set { this.childOrAncestor = value; }
    }
}
