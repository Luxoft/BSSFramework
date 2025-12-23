using Framework.Persistent.Mapping;

namespace SampleSystem.Domain;

[View]
public class BusinessUnitToAncestorChildView : AuditPersistentDomainObjectBase
{
    private BusinessUnit source;

    private BusinessUnit childOrAncestor;

    public virtual BusinessUnit Source
    {
        get { return this.source; }
        set { this.source = value; }
    }

    public virtual BusinessUnit ChildOrAncestor
    {
        get { return this.childOrAncestor; }
        set { this.childOrAncestor = value; }
    }
}
