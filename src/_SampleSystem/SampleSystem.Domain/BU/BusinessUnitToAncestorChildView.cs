using Framework.Database.Mapping;

namespace SampleSystem.Domain.BU;

[View]
public class BusinessUnitToAncestorChildView : AuditPersistentDomainObjectBase
{
    private BusinessUnit childOrAncestor = null!;

    private BusinessUnit source = null!;


    public virtual BusinessUnit ChildOrAncestor => this.childOrAncestor;

    public virtual BusinessUnit Source => this.source;
}

