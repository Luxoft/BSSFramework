using Framework.Database.Mapping;

namespace SampleSystem.Domain.Directories;

[View]
public class LocationToAncestorChildView : AuditPersistentDomainObjectBase
{
    private Location childOrAncestor = null!;

    private Location source = null!;


    public virtual Location ChildOrAncestor => this.childOrAncestor;

    public virtual Location Source => this.source;
}

