using Framework.Persistent;
using Framework.Persistent.Mapping;

namespace SampleSystem.Domain;

[View]
public class LocationToAncestorChildView : AuditPersistentDomainObjectBase, IHierarchicalToAncestorOrChildLink<Location, Guid>
{
    private Location childOrAncestor;

    private Location source;


    public virtual Location ChildOrAncestor => this.childOrAncestor;

    public virtual Location Source => this.source;
}
