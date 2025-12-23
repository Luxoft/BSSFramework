using Framework.Persistent.Mapping;

namespace SampleSystem.Domain;

[View]
public class ManagementUnitToAncestorChildView : AuditPersistentDomainObjectBase
{
    private ManagementUnit source;

    private ManagementUnit childOrAncestor;

    public virtual ManagementUnit Source
    {
        get { return this.source; }
        set { this.source = value; }
    }

    public virtual ManagementUnit ChildOrAncestor
    {
        get { return this.childOrAncestor; }
        set { this.childOrAncestor = value; }
    }
}
