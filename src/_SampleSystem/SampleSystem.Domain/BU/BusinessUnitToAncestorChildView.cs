using Framework.Database.Mapping;

namespace SampleSystem.Domain.BU;

[View]
public class BusinessUnitToAncestorChildView : AuditPersistentDomainObjectBase
{
    private BusinessUnit childOrAncestor;

    private BusinessUnit source;


    public virtual BusinessUnit ChildOrAncestor => this.childOrAncestor;

    public virtual BusinessUnit Source => this.source;
}
